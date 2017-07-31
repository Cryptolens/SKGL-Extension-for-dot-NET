﻿using Cryptolens.SKM.Auth;
using Newtonsoft.Json;
using SKM.V3;
using SKM.V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using SKGL;

namespace User_Login_Auth_Example
{
    class Program
    {
        static void Main(string[] args)
        {

            // This is found at https://serialkeymanager.com/User/Security
            var RSAPublicKey = new RSACryptoServiceProvider(2048);
            RSAPublicKey.FromXmlString("<RSAKeyValue><Modulus>sGbvxwdlDbqFXOMlVUnAF5ew0t0WpPW7rFpI5jHQOFkht/326dvh7t74RYeMpjy357NljouhpTLA3a6idnn4j6c3jmPWBkjZndGsPL4Bqm+fwE48nKpGPjkj4q/yzT4tHXBTyvaBjA8bVoCTnu+LiC4XEaLZRThGzIn5KQXKCigg6tQRy0GXE13XYFVz/x1mjFbT9/7dS8p85n8BuwlY5JvuBIQkKhuCNFfrUxBWyu87CFnXWjIupCD2VO/GbxaCvzrRjLZjAngLCMtZbYBALksqGPgTUN7ZM24XbPWyLtKPaXF2i4XRR9u6eTj5BfnLbKAU5PIVfjIS+vNYYogteQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");

            var authRequest = UserLoginAuth.GetLicenseKeys(SKGL.SKM.getMachineCode(SKGL.SKM.getSHA256), "WyI0NzUiLCJ5aGQvNC9xNU80eEhyWld1UGZQN3d6TytMM0dUV2xrT1VBUlBKY3d6Il0=", "Artem", 5, RSAPublicKey.ExportParameters(false), null, new RSACryptoServiceProvider(2048));

            if (authRequest.error == null)
            {
                var data = JsonConvert.DeserializeObject<GetLicenseKeysResult>(authRequest.jsonResult);

                var licenses = JsonConvert.DeserializeObject<List<KeyInfoResult>>(System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(data.ActivatedMachineCodes)));

                licenses.Where(x => x.LicenseKey.ProductId == 2);

            }
            else
            {
                Console.WriteLine("An error occurred.");
            }


        }
    }


    public class GetLicenseKeysResult : BasicResult
    {
        public string Results { get; set; }
        public string ActivatedMachineCodes { get; set; }
        public string Signature { get; set; }

    }
}
