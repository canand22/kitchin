﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KitchIn.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class WEB {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal WEB() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KitchIn.Resources.WEB", typeof(WEB).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirm Password must be the same as Password.
        /// </summary>
        public static string Model_Validation_Account_ConfirmPassword {
            get {
                return ResourceManager.GetString("Model_Validation_Account_ConfirmPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The email not empty.
        /// </summary>
        public static string Model_Validation_Account_EmailNotEmpty {
            get {
                return ResourceManager.GetString("Model_Validation_Account_EmailNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The email must no more than 125 characters.
        /// </summary>
        public static string Model_Validation_Account_EmailSize {
            get {
                return ResourceManager.GetString("Model_Validation_Account_EmailSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user name must be at least 2 and no more than 25 characters.
        /// </summary>
        public static string Models_Validation_Account_LoginNameSize {
            get {
                return ResourceManager.GetString("Models_Validation_Account_LoginNameSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user name not empty.
        /// </summary>
        public static string Models_Validation_Account_LoginNotEmpty {
            get {
                return ResourceManager.GetString("Models_Validation_Account_LoginNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user password not empty.
        /// </summary>
        public static string Models_Validation_Account_PasswordNotEmpty {
            get {
                return ResourceManager.GetString("Models_Validation_Account_PasswordNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user name must be at least 6 and no more than 25 characters.
        /// </summary>
        public static string Models_Validation_Account_PasswordSize {
            get {
                return ResourceManager.GetString("Models_Validation_Account_PasswordSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current password is incorrect or the new password is invalid..
        /// </summary>
        public static string ModelState_ChangePasswordError {
            get {
                return ResourceManager.GetString("ModelState_ChangePasswordError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid login or password. Try again.
        /// </summary>
        public static string ModelState_InvaliLoginPassword {
            get {
                return ResourceManager.GetString("ModelState_InvaliLoginPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error logon.
        /// </summary>
        public static string ModelState_LoginError {
            get {
                return ResourceManager.GetString("ModelState_LoginError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid old password.
        /// </summary>
        public static string ModelState_OldPasswordError {
            get {
                return ResourceManager.GetString("ModelState_OldPasswordError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user already exists.
        /// </summary>
        public static string ModelState_UserAlreadyExists {
            get {
                return ResourceManager.GetString("ModelState_UserAlreadyExists", resourceCulture);
            }
        }
    }
}
