using AutoMapper;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Extensions;
using ShopProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using Shopproject.Common;
using System.Globalization;

namespace ShopProject.Web.Controllers
{
    public class ContactController : Controller
    {
        private IContactDetailService _contactDetailService;
        private IFeedbackService _feedbackService;
        public ContactController(IContactDetailService contactDetailService, IFeedbackService feedbackService)
        {
            _contactDetailService = contactDetailService;
            _feedbackService = feedbackService;
        }
        // GET: Contact
        public ActionResult Index()
        {
            FeedbackViewModel viewModel = new FeedbackViewModel();
            viewModel.ContactDetail = GetDetail();
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "RegistrationCaptcha", "Mã xác nhận không đúng")]
        public ActionResult SendFeedback(FeedbackViewModel feedbackVm)
        {
            if(ModelState.IsValid)
            {
                var feedback = new Feedback();
                feedback.UpdateFeedback(feedbackVm);
                feedback.Status = true;               
                _feedbackService.Create(feedback);
                _feedbackService.Save();

                ViewData["SuccessMsg"] = "Gửi phản hồi thành công";

                string content = System.IO.File.ReadAllText(Server.MapPath("/assets/client/template/contact_template.html"));
                content = content.Replace("{{Name}}", feedbackVm.Name);
                content = content.Replace("{{Email}}", feedbackVm.Email);
                content = content.Replace("{{Message}}", feedbackVm.Message);
                var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                MailHelper.SendMail(adminEmail, "Thông tin liên hệ từ website", content);

                ModelState["Name"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
                ModelState["Email"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
                ModelState["Message"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
            }
            feedbackVm.ContactDetail = GetDetail();
            return View("Index", feedbackVm);

        }

        public ContactDetailViewModel GetDetail()
        {
            var contact = _contactDetailService.GetDeFaultContact();
            var contactVm = Mapper.Map<ContactDetail, ContactDetailViewModel>(contact);
            return contactVm;
        }
    }
}