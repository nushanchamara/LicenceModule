using System;

namespace LicenceModule
{
    public static class LicenceCheck
    {

        public static LicenceValidity CheckValidLicence(string key, string serverDate, string client)
        {
            var licenceValidity = new LicenceValidity { IsCheckProcessSuccess = false };

            string ParseLevel = "Initial";

            try
            {
                if (key == null || key == "")
                {
                    throw new Exception("Licence Key Not Applied");
                }

                if(serverDate == null || serverDate == "")
                {
                    throw new Exception("Server date Error");
                }

                if(client == null || client == "")
                {
                    throw new Exception("Client code Error");
                }


                var splitServerDate = serverDate.Split('-');
                var serverDateActual = new DateTime(Convert.ToInt16(splitServerDate[2]), Convert.ToInt16(splitServerDate[1]),
                    Convert.ToInt16(splitServerDate[0]), 23, 59, 59);

                ParseLevel = "Key Decode Stage 1";

                var keyWithNoDash = key.Replace("-", string.Empty);
                var keyArray = keyWithNoDash.ToCharArray();

                ParseLevel = "Key Decode Stage 2";
                var asciiSum = keyArray[8] + keyArray[20];
                var asciiSumString = asciiSum.ToString();
                var checkSum1 = asciiSumString.Substring(0, 1);
                var checkSum2 = asciiSumString.Substring(asciiSumString.Length - 1, 1);
                ParseLevel = "Key Decode Stage 3";
                var fullAsciiSum = keyArray[1] + keyArray[3] + keyArray[4] + keyArray[6] + keyArray[7] + keyArray[8] +
                                   keyArray[10] + keyArray[12] + keyArray[13] + keyArray[15] + keyArray[16] + keyArray[18] +
                                   keyArray[19] + keyArray[20] + keyArray[22] + keyArray[25] + keyArray[28];
                var fullAsciiSumString = fullAsciiSum.ToString();
                var fullCkeckSum1 = fullAsciiSumString.Substring(0, 1);
                var fullCkeckSum2 = fullAsciiSumString.Substring(fullAsciiSumString.Length - 1, 1);
                ParseLevel = "Key Decode Stage 4";
                var clientShortArray = client.ToCharArray();
                var clientShortAsciiSum = clientShortArray[0] + clientShortArray[1];
                var clientShortAsciiSumString = clientShortAsciiSum.ToString();
                var clientShortAsciiSumLast = clientShortAsciiSumString.Substring(clientShortAsciiSumString.Length - 1, 1);
                ParseLevel = "Key Decode Stage 5";
                var clientShortAsciiSumLastInt = Convert.ToInt16(clientShortAsciiSumLast);
                var clientShortChecksum = Math.Abs(clientShortAsciiSumLastInt - Convert.ToInt16(keyArray[6].ToString()));

                var clientIdxOne = clientShortArray[0];

                ParseLevel = "Key Decode Stage 6";

                var clientLongCheckSum = clientShortAsciiSum * Convert.ToInt16(string.Concat(keyArray[26].ToString(), keyArray[27].ToString()));
                var clientLongCheckSumString = clientLongCheckSum.ToString();
                var clientLongCheckSum1 = clientLongCheckSumString.Substring(clientLongCheckSumString.Length - 2, 1);
                var clientLongCheckSum2 = clientLongCheckSumString.Substring(clientLongCheckSumString.Length - 1, 1);

                ParseLevel = "Key Decode Stage 7";

                var a = Convert.ToInt16(fullCkeckSum2) == Convert.ToInt16(keyArray[26].ToString());
                var b = Convert.ToInt16(fullCkeckSum1) == Convert.ToInt16(keyArray[27].ToString());
                var c = Convert.ToInt16(checkSum2) == Convert.ToInt16(keyArray[2].ToString());
                var d = Convert.ToInt16(checkSum1) == Convert.ToInt16(keyArray[14].ToString());
                var e = clientShortChecksum == Convert.ToInt16(keyArray[18].ToString());
                var f = Convert.ToInt16(clientLongCheckSum1) == Convert.ToInt16(keyArray[21].ToString());
                var g = Convert.ToInt16(clientLongCheckSum2) == Convert.ToInt16(keyArray[9].ToString());
                var h = ExtensionMethods.ValidateClient(Convert.ToInt32(Char.GetNumericValue(keyArray[6])), clientShortArray[0], keyArray[0]);

                ParseLevel = "Key Decode Stage 8";

                if (Convert.ToInt16(fullCkeckSum2) == Convert.ToInt16(keyArray[26].ToString()) &&
                    Convert.ToInt16(fullCkeckSum1) == Convert.ToInt16(keyArray[27].ToString()) &&
                    Convert.ToInt16(checkSum2) == Convert.ToInt16(keyArray[2].ToString()) &&
                    Convert.ToInt16(checkSum1) == Convert.ToInt16(keyArray[14].ToString()) &&
                    clientShortChecksum == Convert.ToInt16(keyArray[18].ToString()) &&
                    Convert.ToInt16(clientLongCheckSum1) == Convert.ToInt16(keyArray[21].ToString()) &&
                    Convert.ToInt16(clientLongCheckSum2) == Convert.ToInt16(keyArray[9].ToString()) )
                {
                    licenceValidity.IsValid = true;
                }
                else
                {
                    licenceValidity.IsValid = false;
                    licenceValidity.IsCheckProcessSuccess = true;
                    return licenceValidity;
                }

                ParseLevel = "Key Decode Stage 9";

                var expireKey = 480402 * asciiSum;
                var expireKeyString = expireKey.ToString();
                var expireKeySub = expireKeyString.Substring(expireKeyString.Length - 5, 5);
                ParseLevel = "Key Decode Stage 10";
                var expiryKeyYear =
                    string.Concat((Convert.ToInt16(keyArray[29].ToString()) - Convert.ToInt16(keyArray[6].ToString())).ToString(),
                        Convert.ToInt16(keyArray[24].ToString()).ToString());
                var expiryKeyMonth = string.Concat(Convert.ToInt16(keyArray[11].ToString()).ToString(),
                    Convert.ToInt16(keyArray[5].ToString()).ToString());
                var expiryKeyDay = string.Concat(Convert.ToInt16(keyArray[23].ToString()).ToString(),
                    Convert.ToInt16(keyArray[17].ToString()).ToString());
                ParseLevel = "Key Decode Stage 11";
                var expiryValue = Convert.ToInt64(String.Concat(expiryKeyYear, expiryKeyDay, expiryKeyMonth));
                var expiryDateString = expiryValue - Convert.ToInt64(expireKeySub);

                if (expiryDateString <= 0)
                {
                    licenceValidity.IsValid = false;
                    licenceValidity.IsCheckProcessSuccess = true;
                    return licenceValidity;
                }
                ParseLevel = "Key Decode Stage 12";
                var expiryYear = string.Concat("20", expiryDateString.ToString().Substring(0, 2));
                var expiryMonth = expiryDateString.ToString().Substring(4, 2);
                var expiryDay = expiryDateString.ToString().Substring(2, 2);
                ParseLevel = "Key Decode Stage 13";
                var expiryDate = licenceValidity.ExpiryDate = new DateTime(Convert.ToInt16(expiryYear), Convert.ToInt16(expiryMonth), Convert.ToInt16(expiryDay), 23, 59, 59);

                licenceValidity.IsExpired = (expiryDate - serverDateActual).TotalDays < 0;
                licenceValidity.IsCheckProcessSuccess = true;
                return licenceValidity;
            }
            catch (Exception e)
            {
                licenceValidity.CheckProcessFailedReason = ParseLevel +" @ "+ e.Message;
                return licenceValidity;
            }
        }

        public struct LicenceValidity
        {
            public bool IsCheckProcessSuccess;
            public string CheckProcessFailedReason;
            public bool IsValid;
            public bool IsExpired;
            public DateTime ExpiryDate;
        }
    }
}
