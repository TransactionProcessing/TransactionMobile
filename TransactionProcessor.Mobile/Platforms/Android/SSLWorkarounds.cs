using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionProcessor.Mobile.Platforms.Android
{
    using System.Reflection;
    using System.Reflection.Emit;
    using Java.Net;
    using Java.Security;
    using Java.Security.Cert;
    using Javax.Net.Ssl;
    using Xamarin.Android.Net;

    internal class DangerousTrustProvider : Provider
    {
        private const string TRUST_PROVIDER_ALG = "DangerousTrustAlgorithm";
        private const string TRUST_PROVIDER_ID = "DangerousTrustProvider";

        public DangerousTrustProvider() : base(TRUST_PROVIDER_ID, 1, string.Empty)
        {
            var key = "TrustManagerFactory." + DangerousTrustManagerFactory.GetAlgorithm();
            var val = Java.Lang.Class.FromType(typeof(DangerousTrustManagerFactory)).Name;
            Put(key, val);
        }

        public static void Register()
        {
            Provider registered = Security.GetProvider(TRUST_PROVIDER_ID);
            if (null == registered)
            {
                Security.InsertProviderAt(new DangerousTrustProvider(), 1);
                Security.SetProperty("ssl.TrustManagerFactory.algorithm", TRUST_PROVIDER_ALG);
            }
        }

        public class DangerousTrustManager : X509ExtendedTrustManager
        {
            public override void CheckClientTrusted(X509Certificate[] chain,
                                                    string authType,
                                                    Socket socket) {
                // Do nothing
            }

            public override void CheckClientTrusted(X509Certificate[] chain,
                                                    string authType,
                                                    SSLEngine engine) {
                // Do nothing
            }

            public override void CheckClientTrusted(X509Certificate[] chain,
                                                    string authType) {
                // Do nothing
            }

            public override void CheckServerTrusted(X509Certificate[] chain,
                                                    string authType,
                                                    Socket socket) {
                // Do nothing
            }

            public override void CheckServerTrusted(X509Certificate[] chain,
                                                    string authType,
                                                    SSLEngine engine) {
                // Do nothing
            }

            public override void CheckServerTrusted(X509Certificate[] chain,
                                                    string authType) {
                // Do nothing
            }

            public override X509Certificate[] GetAcceptedIssuers() => Array.Empty<X509Certificate>();
        }

        public class DangerousTrustManagerFactory : TrustManagerFactorySpi
        {
            protected override void EngineInit(IManagerFactoryParameters mgrparams) {
                // Do nothing
            }

            protected override void EngineInit(KeyStore keystore) {
                // Do nothing
            }

            protected override ITrustManager[] EngineGetTrustManagers() => new ITrustManager[] { new DangerousTrustManager() };

            public static string GetAlgorithm() => TRUST_PROVIDER_ALG;
        }
    }

    static class DangerousAndroidMessageHandlerEmitter
    {
        private static Assembly _emittedAssembly = null;

        public static void Register(string handlerTypeName = "DangerousAndroidMessageHandler", string assemblyName = "DangerousAndroidMessageHandler")
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                if (e.Name == assemblyName)
                {
                    if (_emittedAssembly == null)
                    {
                        _emittedAssembly = Emit(handlerTypeName, assemblyName);
                    }

                    return _emittedAssembly;
                }
                return null;
            };
        }

        private static AssemblyBuilder Emit(string handlerTypeName, string assemblyName)
        {
            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(assemblyName);

            DefineDangerousAndroidMessageHandler(module, handlerTypeName);

            return assembly;
        }

        private static void DefineDangerousAndroidMessageHandler(ModuleBuilder module, string handlerTypeName)
        {
            var typeBuilder = module.DefineType(handlerTypeName, TypeAttributes.Public);
            typeBuilder.SetParent(typeof(AndroidMessageHandler));
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            var methodBuilder = typeBuilder.DefineMethod(
                "GetSSLHostnameVerifier",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(IHostnameVerifier),
                new[] { typeof(HttpsURLConnection) }
            );

            var generator = methodBuilder.GetILGenerator();
            generator.Emit(OpCodes.Call, typeof(DangerousHostNameVerifier).GetMethod("Create"));
            generator.Emit(OpCodes.Ret);

            typeBuilder.CreateType();
        }
    }

    public class DangerousHostNameVerifier : Java.Lang.Object, IHostnameVerifier
    {
        public bool Verify(string hostname, ISSLSession session)
        {
            return true;
        }

        public static IHostnameVerifier Create() => new DangerousHostNameVerifier();
    }
}
