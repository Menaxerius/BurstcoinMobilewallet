using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BNWallet
{
    class BNWalletAPI
    {

        private HttpClient client;
        private string BaseAddr { get; set; }
        public bool IsSuccessfull { get; set; }
        public string ErrMesg { get; set; }
        public string ErrCode { get; set; }
        FormUrlEncodedContent fp = null;

        public BNWalletAPI()
        {
            client = new HttpClient();
            BaseAddr = "https://wallet1.burstnation.com:8125";
            client.BaseAddress = new System.Uri(BaseAddr);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ErrMesg = "";
            ErrCode = "";
        }

        public GetAccountIDResult getAccountID(string secretPhrase, string publicKey)
        {
            GetAccountIDResult GAIR = new GetAccountIDResult();
            BNWalletAPIClasses.ErrorCodes er;

            fp = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("requestType","getAccountId"),
                new KeyValuePair<string, string>("secretPhrase",secretPhrase),
                new KeyValuePair<string, string>("publicKey",publicKey)

            });

            HttpResponseMessage resp = client.PostAsync("burst", fp).Result;
            string respStr = resp.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(respStr))
            {
                if (respStr.Contains("\"errorCode\":"))
                {
                    er = JsonConvert.DeserializeObject<BNWalletAPIClasses.ErrorCodes>(respStr);
                    GAIR.success = false;
                    GAIR.errorMsg = er.errorDescription;
                    //GAIR.errorMsg = fp;
                }
                else
                {
                    BNWalletAPIClasses.GetAccountIDResponse gair = JsonConvert.DeserializeObject<BNWalletAPIClasses.GetAccountIDResponse>(respStr);
                    GAIR.success = true;
                    GAIR.accountRS = gair.accountRS;
                    GAIR.account = gair.account;
                }
            }
            else
            {
                GAIR.success = false;
                GAIR.errorMsg = "Receive blank response from API call";
            }
            return GAIR;

        }

        public GetAccountResult getAccount(string WalletID)
        {
            GetAccountResult GAR = new GetAccountResult();
            BNWalletAPIClasses.ErrorCodes er;

            fp = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("requestType","getAccount"),
                new KeyValuePair<string, string>("account",WalletID)

            });

            HttpResponseMessage resp = client.PostAsync("burst" , fp).Result;
            string respStr = resp.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(respStr))
            {
                if (respStr.Contains("\"errorCode\":"))
                {
                    er = JsonConvert.DeserializeObject<BNWalletAPIClasses.ErrorCodes>(respStr);
                    GAR.success = false;
                    GAR.errorMsg = er.errorDescription;
                   
                }
                else
                {
                    BNWalletAPIClasses.GetAccountResponse gar = JsonConvert.DeserializeObject<BNWalletAPIClasses.GetAccountResponse>(respStr);
                    GAR.success = true;
                    GAR.accountRS = gar.accountRs;
                    GAR.name = gar.name;
                    GAR.balanceNQT = gar.balanceNQT;

                }
            }
            else
            {
                GAR.success = false;
                GAR.errorMsg = "Receive blank response from API call";
            }
            return GAR;

        }

        public GetsendMoneyResult sendMoney(string BurstAddress,string amountNQT,string feeNQT,string secretPhrase,string message,bool encrypmf)
        {
            GetsendMoneyResult SMR = new GetsendMoneyResult();
            BNWalletAPIClasses.ErrorCodes er;

            Dictionary<string, string>  fpDict = new Dictionary<string,string>()
            {
                { "requestType","sendMoney" },
                { "secretPhrase",secretPhrase },
                { "recipient",BurstAddress },
                { "amountNQT",amountNQT },
                { "feeNQT","100000000" },
                { "deadline","60" }

            };
            if (encrypmf)
                fpDict.Add("messageToEncrypt", message);
            else
                fpDict.Add("message", message);
            fp = new FormUrlEncodedContent(fpDict);

            HttpResponseMessage resp = client.PostAsync("burst", fp).Result;
            string respStr = resp.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(respStr))
            {
                if (respStr.Contains("\"errorCode\":"))
                {
                    er = JsonConvert.DeserializeObject<BNWalletAPIClasses.ErrorCodes>(respStr);
                    SMR.success = false;
                    SMR.errorMsg = er.errorDescription;
                    //GAIR.errorMsg = fp;
                }
                else
                {
                    BNWalletAPIClasses.sendMoneyResponse smr = JsonConvert.DeserializeObject<BNWalletAPIClasses.sendMoneyResponse>(respStr);
                    SMR.success = true;
                    SMR.signatureHash = smr.signatureHash;
                    SMR.transaction = smr.transaction;
                }
            }
            else
            {
                SMR.success = false;
                SMR.errorMsg = "Receive blank response from API call";
            }
            return SMR;

        }

        

        public GetTransactionResult getTransaction(string transaction)
        {
            GetTransactionResult GTR = new GetTransactionResult();
            BNWalletAPIClasses.ErrorCodes er;

            fp = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("requestType","getTransaction"),
                new KeyValuePair<string, string>("transaction",transaction)
            });

            HttpResponseMessage resp = client.PostAsync("burst", fp).Result;
            string respStr = resp.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(respStr))
            {
                if (respStr.Contains("\"errorCode\":"))
                {
                    er = JsonConvert.DeserializeObject<BNWalletAPIClasses.ErrorCodes>(respStr);
                    GTR.success = false;
                    GTR.errormsg = er.errorDescription;
                    //GAIR.errorMsg = fp;
                }
                else
                {
                    BNWalletAPIClasses.getTransactionResponse gtr = JsonConvert.DeserializeObject<BNWalletAPIClasses.getTransactionResponse>(respStr);
                    GTR.success = true;
                    GTR.feeNQT = gtr.feeNQT;
                    GTR.senderRS = gtr.senderRS;
                    GTR.amountNQT = gtr.amountNQT;
                    GTR.recipientRS = gtr.recipientRS;
                    GTR.ecBlockHeight = gtr.ecBlockHeight;
                }
            }
            else
            {
                GTR.success = false;
                GTR.errormsg = "Receive blank response from API call";
            }
            return GTR;

        }

    }

    public class GetAccountIDResult
    {
        public bool success { get; set; }
        public string accountRS { get; set; }
        public string account { get; set; }
        public string errorMsg { get; set; }

        public GetAccountIDResult()
        {
            success = false;
            accountRS = "";
            account = "";
            errorMsg = "";

        }
    }

    public class GetAccountResult
    {
        public bool success { get; set; }
        public string accountRS { get; set; }
        public string name { get; set; }
        public string balanceNQT { get; set; }
        public string errorMsg { get; set; }

        public GetAccountResult()
        {
            success = false;
            accountRS = "";
            name = "";
            balanceNQT = "";
            errorMsg = "";

        }
    }

    public class GetsendMoneyResult
    {
        public bool success { get; set; }
        public string signatureHash { get; set; }
        public string transaction { get; set; }
        public string errorMsg { get; set; }

        public GetsendMoneyResult()
        {
            success = false;
            signatureHash = "";
            transaction = "";
            errorMsg = "";
        }
    }

    public class GetTransactionResult
    {
        public bool success { get; set; }
        public string feeNQT { get; set; }
        public string senderRS { get; set; }
        public string amountNQT { get; set; }
        public string recipientRS { get; set; }
        public string ecBlockHeight { get; set; }
        public string errormsg { get; set; }

        public GetTransactionResult()
        {
            success = false;
            feeNQT = "";
            senderRS = "";
            amountNQT = "";
            recipientRS = "";
            ecBlockHeight = "";
            errormsg = "";
        }


            
    }
}