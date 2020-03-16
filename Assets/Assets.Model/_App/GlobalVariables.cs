
namespace Assets.Model {
    public class GlobalVariables {
        public const string ExceptionSource = "Custom";
        public const string SystemGeneratedMessage = "SystemGeneratedMessage";
        public const string UnknownError = "An unknown error happened.";
        public static string[] ClientUnsafeKeywords = { "javascript", "vbscript", "script" };
        public static string[] SqlUnsafeKeywords = { "shutdown", "exec", "having", "union", "select", "insert", "update", "delete", "drop", "truncate", "script" };
    }

    public class MethodName {
        public const string ClientStart = "ClientStart";
        public const string ExecuteQuery = "ExecuteQuery";
        public const string ReformUpdater = "ReformUpdater";
        public const string UpdateSenderReceiver = "UpdateSenderReceiver";
        public const string UpdateReplicator = "UpdateReplicator";
    }
}
