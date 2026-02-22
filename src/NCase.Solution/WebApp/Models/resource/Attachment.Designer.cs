namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class Attachment {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Attachment() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.Attachment", typeof(Attachment).Assembly);
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
    public static string Id {
            get {
                return ResourceManager.GetString("Id", resourceCulture);
            }
    }
    public static string CaseId {
            get {
                return ResourceManager.GetString("CaseId", resourceCulture);
            }
    }
    public static string Description {
            get {
                return ResourceManager.GetString("Description", resourceCulture);
            }
    }
    public static string DocId {
            get {
                return ResourceManager.GetString("DocId", resourceCulture);
            }
    }
    public static string Type {
            get {
                return ResourceManager.GetString("Type", resourceCulture);
            }
    }
    public static string Path {
            get {
                return ResourceManager.GetString("Path", resourceCulture);
            }
    }
    public static string Ext {
            get {
                return ResourceManager.GetString("Ext", resourceCulture);
            }
    }
    public static string ExpireDate {
            get {
                return ResourceManager.GetString("ExpireDate", resourceCulture);
            }
    }
 }
}