using Agri_Energy_Connect.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Agri_Energy_Connect.Controllers
{
   
        public class HomeController : Controller
        {
            FirebaseAuthProvider auth;
        private readonly Poe2Context _context;

        public HomeController(Poe2Context context)
            {
                auth = new FirebaseAuthProvider(
                                new FirebaseConfig(""));
            _context = context;
            }

        //This view is returned when the user chooses to view the privacy policy of Agri-ENergy Connect.
            public IActionResult Privacy()
            {
                return View();
            }


        //This view is returned if the user's request to be a farmer is pending. 
        public IActionResult RequestPending() {
        return View();
        }

        //This view is returned if the user's request to be a farmer is denied. 
        public IActionResult RequestDenied()
        {
            return View();
        }

        //This view is returned after a user requests to register to be a farmer. 
        public IActionResult RequestToRegister()
        {
            return View();
        }

        //This is the view of all the users.
        public IActionResult Index()
            {
            return View();
            }

        //THis is the view that is returned to the user when they request to register.
        public IActionResult Registration()
            {
            ViewData["UserRole"] = new SelectList(_context.Roles, "UserRole", "UserRole");
            return View();
            }
        public IActionResult RegisterAdmin()
        {
            ViewData["UserRole"] = new SelectList(_context.Roles, "UserRole", "UserRole");
            return View();
        }
        //This is the view that is returned to the user when they attempt to sign in.
        public IActionResult SignIn()
            {
                return View();
            }


        //The following code was adapted from Varsity College Durban North's GitHub
        //https://github.com/PROG7311-VCDN-2024/MathApp/blob/master/MathApp/Controllers/AuthController.cs
        //ebadamZA
        //https://github.com/ebadamZA
        //The following code was adapted:

        //[HttpPost]
        //public async Task<IActionResult> Register(LoginModel login)
        //{
        //    try
        //    {
        //        await auth.CreateUserWithEmailAndPasswordAsync(login.Email, login.Password);

        //        var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);
        //        string currentUserId = fbAuthLink.User.LocalId;

        //        if (currentUserId != null)
        //        {
        //            HttpContext.Session.SetString("currentUser", currentUserId);
        //            return RedirectToAction("Calculate", "Math");
        //        }
        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        var firebaseEx = JsonConvert.DeserializeObject<FirebaseErrorModel>(ex.ResponseData);
        //        ModelState.AddModelError(String.Empty, firebaseEx.error.message);
        //        return View(login);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(String.Empty, ex.Message);
        //        return View(login);
        //    }

        //    return View();

        //}

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginModel login)
        //{
        //    try
        //    {
        //        var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);
        //        string currentUserId = fbAuthLink.User.LocalId;

        //        if (currentUserId != null)
        //        {
        //            HttpContext.Session.SetString("currentUser", currentUserId);
        //            return RedirectToAction("Calculate", "Math");
        //        }

        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        var firebaseEx = JsonConvert.DeserializeObject<FirebaseErrorModel>(ex.ResponseData);
        //        ModelState.AddModelError(String.Empty, firebaseEx.error.message);
        //        Utils.AuthLogger.Instance.LogError(firebaseEx.error.message + " - User: " + login.Email + " - IP: " + HttpContext.Connection.RemoteIpAddress
        //            + " - Browser: " + Request.Headers.UserAgent);
        //        return View(login);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(String.Empty, ex.Message);
        //        return View(login);
        //    }

        //    return View();
        //}

        //[HttpGet]
        //public IActionResult LogOut()
        //{
        //    HttpContext.Session.Remove("currentUser");
        //    return RedirectToAction("Login");
        //}


        [HttpPost]
            //A new user is created and their log in details are stored in firebase and in their personal details are stored in the database.
            //This method is for users who are applying to be farmer.
            public async Task<IActionResult> Registration(LoginModel loginModel)
            {
            //The user role is set to requested as this iuser has not yet been approved to be a farmer. 
            loginModel.UserRole = "Requested";
            //The new user is created and logged in 
                try
                {
                    
                    await auth.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password, loginModel.ConfirmPassword);
                   
                    var fbAuthLink = await auth
                                    .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                    string token = fbAuthLink.FirebaseToken;
                    string currentUserId = fbAuthLink.User.LocalId;
                //The user's id and users' role are saved in session variables. 
                    HttpContext.Session.SetString("userId", fbAuthLink.User.LocalId);
                HttpContext.Session.SetString("userRole", loginModel.UserRole);
                    var user = HttpContext.Session.GetString("theUser");

                    //The data is saved in session variables and in the database. 
                    if (token != null && user == null)
                    {
                        HttpContext.Session.SetString("_UserToken", token);

                        HttpContext.Session.SetString("theUser", loginModel.Email);



                        using (SqlConnection con = new SqlConnection("Data Source=NAIYAH;Initial Catalog=POE2;Integrated Security=True;Trust Server Certificate=True"))
                        {
                            using (SqlCommand cmd = new SqlCommand("CREATE_USER", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@UserID", currentUserId);
                                cmd.Parameters.AddWithValue("@Email", loginModel.Email);
                                cmd.Parameters.AddWithValue("@FullName", loginModel.FullName);
                                cmd.Parameters.AddWithValue("@UserRole", loginModel.UserRole);
                                cmd.Parameters.AddWithValue("@status", "INSERT");
                                con.Open();
                                ViewData["result"] = cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        //The user is redirected to a view that provides the turn around time for the user's registration request to be approved. 
                        return RedirectToAction("RequestToRegister");
                    }


                }

            //If there are any errors with the firebase, then this catch will deplay the error message and return a view. 
                catch (FirebaseAuthException ex)
                {
                    var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                    ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                    return View(loginModel);
                }

                return View();

            }

        [HttpPost]
        //A new user is created and their log in details are stored in firebase and in their personal details are stored in the database.
        //This method is for users who are applying to be farmer.
        public async Task<IActionResult> RegisterAdmin(LoginModel loginModel)
        {
            //The user role is set to admin as this iuser will be an admin. 
            loginModel.UserRole = "Admin";
            //The new user is created
            try
            {
                var user = HttpContext.Session.GetString("userId");
                var fbAuthLink = await auth.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password, loginModel.ConfirmPassword);

                //The data is then saved in the database.
    using (SqlConnection con = new SqlConnection("Data Source=NAIYAH;Initial Catalog=POE2;Integrated Security=True;Trust Server Certificate=True"))
                    {
                        using (SqlCommand cmd = new SqlCommand("CREATE_USER", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserID", fbAuthLink.User.LocalId);
                            cmd.Parameters.AddWithValue("@Email", loginModel.Email);
                            cmd.Parameters.AddWithValue("@FullName", loginModel.FullName);
                            cmd.Parameters.AddWithValue("@UserRole", loginModel.UserRole);
                            cmd.Parameters.AddWithValue("@status", "INSERT");
                            con.Open();
                            ViewData["result"] = cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                HttpContext.Session.SetString("userId", user);
                //The user is redirected to a view that provides the turn around time for the user's registration request to be approved. 
                return RedirectToAction("Index");
                

            }

            //If there are any errors with the firebase, then this catch will deplay the error message and return a view. 
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(loginModel);
            }

            return View();

        }

        //This method signs in either an existing farrmer or admin user. 

        [HttpPost]
            public async Task<IActionResult> SignIn(LoginModel loginModel)
            {
            try
            {
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                string token = fbAuthLink.FirebaseToken;
                
                if (token != null)
                {
                    //Data is stored in session variables.
                    HttpContext.Session.SetString("_UserToken", token);
                    HttpContext.Session.SetString("userId", fbAuthLink.User.LocalId);
                    HttpContext.Session.SetString("theUser", loginModel.Email);


                    //The user's role is selected from the database
                    using (SqlConnection connection = new SqlConnection("Data Source=NAIYAH;Initial Catalog=POE2;Integrated Security=True;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string query = "Select UserRole from UserAccount where Email ='" + loginModel.Email + "'";

                        SqlCommand command1 = new SqlCommand(query, connection);
                        SqlDataReader reader1 = command1.ExecuteReader();


                        //If the user role, it is recorded in variables and used for calculations.
                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {

                                HttpContext.Session.SetString("userRole", reader1["UserRole"].ToString());
                            }
                        }
                        //The user role is retrieved from the session variable.
                        var role = HttpContext.Session.GetString("userRole");

                        

                        //If the user role is denied then the user will be riedirected to the request denied view.
                         if (role.Equals("Denied")) {
                            return RedirectToAction("RequestDenied");
                        }

                        //If the user role is requested then the user will be redirectd to the request pending view. 
                        else if (role.Equals("Requested"))
                        {
                            return RedirectToAction("RequestPending");
                        }

                        //If the user role is not denied or requested then the user will be redirected to the index
                        if (!role.Equals("Denied") || !role.Equals("Requested"))
                        {
                            return RedirectToAction("Index");
                        }
                    }


                }
            }
            //If there are any exceptions with the fire base then the error message is displayed.
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(loginModel);
            }
            //The view is returned. 
                return View();
            }

        //This method logs the user out. 
            public IActionResult LogOut()
            {
            //Session variables are removed and the user is logged out. 
            HttpContext.Session.Remove("_UserToken");
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("theUser");
            HttpContext.Session.Remove("userRole");
            return RedirectToAction("SignIn");
            }
        }
}
