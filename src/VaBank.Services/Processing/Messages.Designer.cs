﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VaBank.Services.Processing {
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
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VaBank.Services.Processing.Messages", typeof(Messages).Assembly);
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
        ///   Looks up a localized string similar to Запрещен доступ к карте..
        /// </summary>
        public static string CardAccessDenied {
            get {
                return ResourceManager.GetString("CardAccessDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Карта заблокирована..
        /// </summary>
        public static string CardBlocked {
            get {
                return ResourceManager.GetString("CardBlocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Истек срок действия карты..
        /// </summary>
        public static string CardExpired {
            get {
                return ResourceManager.GetString("CardExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Превышены лимиты по карте..
        /// </summary>
        public static string CardLimitsExceeded {
            get {
                return ResourceManager.GetString("CardLimitsExceeded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сумма перевода должна превышать {MinimalAmount} {CurrencyISOName}..
        /// </summary>
        public static string CardTransferSmallAmount {
            get {
                return ResourceManager.GetString("CardTransferSmallAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не найдена карта получателя..
        /// </summary>
        public static string DestinationCardNotFound {
            get {
                return ResourceManager.GetString("DestinationCardNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка обработки перевода: {0}..
        /// </summary>
        public static string TransferFailed {
            get {
                return ResourceManager.GetString("TransferFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Денежный перевод №{0} не может быть выполнен. Обратитесь в службу поддержки..
        /// </summary>
        public static string TransferFailedUnknownReason {
            get {
                return ResourceManager.GetString("TransferFailedUnknownReason", resourceCulture);
            }
        }
    }
}
