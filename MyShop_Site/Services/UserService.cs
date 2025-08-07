using Microsoft.IdentityModel.Logging;
using Minerets.Shop.Models;
using MyShop_Site.Models.Authentication;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;


namespace MyShop_Site.Services
{
    public class UserService
    {

        //public async Task RegisterAsync()
        //{
        //    //IResponseModel response = await MasterService.RequestMaster<CreateUserResponseModel>("Authentication/Signup", new CreateUserModel()
        //    //{
        //    //    //FullName = FullName,
        //    //    //UserName = UserName,
        //    //    //EmailAddress = Email,
        //    //    //MobileNumber = MobileNumber,
        //    //    //Password = Password,
        //    //});
        //    if (response is CreateUserResponseModel createUserResponseModel)
        //    {
        //        //    userID = createUserResponseModel.ID;
        //        //    Route to Verification Page
        //        //        _ = ((VerificationModel)verificationPage.BindingContext).StartVerification(VerifyOtp, ResendOtp, MobileNumber, Email);
        //        //}
        //    }
        //    else if (response is FailedResponseModel failedResponseModel)
        //    {
        //    }
        //}
    

        // Helper method for password hashing (replace with a strong hashing algorithm in production)
        private string HashPassword(string password)
        {
            // Example using a simple hash, replace with BCrypt or similar for production
            using (var sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Helper method for password verification (replace with a strong hashing algorithm in production)
        private bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            // Example using a simple hash, replace with BCrypt or similar for production
            return HashPassword(providedPassword) == hashedPassword;
        }


    }
}