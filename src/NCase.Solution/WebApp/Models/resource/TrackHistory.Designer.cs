namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class TrackHistory {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TrackHistory() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.TrackHistory", typeof(TrackHistory).Assembly);
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
    public static string Status {
            get {
                return ResourceManager.GetString("Status", resourceCulture);
            }
    }
    public static string Node {
            get {
                return ResourceManager.GetString("Node", resourceCulture);
            }
    }
    public static string NodeStatus {
            get {
                return ResourceManager.GetString("NodeStatus", resourceCulture);
            }
    }
    public static string Owner {
            get {
                return ResourceManager.GetString("Owner", resourceCulture);
            }
    }
    public static string ToUser {
            get {
                return ResourceManager.GetString("ToUser", resourceCulture);
            }
    }
    public static string BeginDate {
            get {
                return ResourceManager.GetString("BeginDate", resourceCulture);
            }
    }
    public static string CompletedDate {
            get {
                return ResourceManager.GetString("CompletedDate", resourceCulture);
            }
    }
    public static string Expires {
            get {
                return ResourceManager.GetString("Expires", resourceCulture);
            }
    }
    public static string DoDate {
            get {
                return ResourceManager.GetString("DoDate", resourceCulture);
            }
    }
    public static string Elapsed {
            get {
                return ResourceManager.GetString("Elapsed", resourceCulture);
            }
    }
    public static string State {
            get {
                return ResourceManager.GetString("State", resourceCulture);
            }
    }
    public static string Comment {
            get {
                return ResourceManager.GetString("Comment", resourceCulture);
            }
    }
    public static string Created {
            get {
                return ResourceManager.GetString("Created", resourceCulture);
            }
    }
 }
}