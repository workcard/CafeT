using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using MathBot.Models;
using MathBot.Managers;

namespace MathBot
{
    [Serializable]
    public class UserProfileForm
    {
        [Prompt("What is your display name? {||}")]
        public string DisplayName;

        [Prompt("What is your first name? {||}")]
        public string FirstName;

        [Prompt("What is your last name? {||}")]
        public string LastName;

        [Prompt("Email ? {||}")]
        public string Email;

        [Prompt("Password ? {||}")]
        public string Password;

        [Prompt("Mobile Phone ? {||}")]
        public string MobilePhone;

        
        // This method 'builds' the form 
        // This method will be called by code we will place
        // in the MakeRootDialog method of the MessagesControlller.cs file
        public static IForm<UserProfileForm> BuildForm()
        {
            OnCompletionAsyncDelegate<UserProfileForm> Save = async (context, state) =>
            {
                UserManager _manager = new UserManager();
                UserProfile _user = new UserProfile();
                _user.Email = context.PrivateConversationData.GetValue<string>("Email");

                await _manager.AddUser(_user);
                await context.PostAsync("Cảm ơn. Bạn đã đăng ký thành công.");
            };

            return new FormBuilder<UserProfileForm>()
                    .Message("Xin chào. Bạn điền các thông tin sau đầy đủ và cẩn thận nhé:")
                    //.OnCompletion(Save)
                    .OnCompletion(async (context, profileForm) =>
                    {
                        // Set BotUserData
                        context.PrivateConversationData.SetValue<bool>(
                            "ProfileComplete", true);
                        context.PrivateConversationData.SetValue<string>(
                            "DisplayName", profileForm.DisplayName);
                        context.PrivateConversationData.SetValue<string>(
                            "Email", profileForm.Email);
                        context.PrivateConversationData.SetValue<string>(
                            "Password", profileForm.Password);
                        context.PrivateConversationData.SetValue<string>(
                            "MobilePhone", profileForm.MobilePhone);
                        context.PrivateConversationData.SetValue<string>(
                            "FirstName", profileForm.FirstName);
                        context.PrivateConversationData.SetValue<string>(
                            "LastName", profileForm.LastName);

                        // Tell the user that the form is complete
                        await Save(context, profileForm);
                        
                    })
                    .Build();
        }
    }
}