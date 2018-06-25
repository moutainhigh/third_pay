﻿using System;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using DinpayRSAAPI.COM.Dinpay.RsaUtils;
using System.Security.Cryptography;


namespace testOrderQuery
{
    public partial class getXmlData : System.Web.UI.Page
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
				/////////////////////////////////接收表单提交参数//////////////////////////////////////

                string merchant_code = Request.Form["merchant_code"].ToString().Trim();

                string service_type = Request.Form["service_type"].ToString().Trim();

                string sign_type = Request.Form["sign_type"].ToString().Trim();

                string interface_version = Request.Form["interface_version"].ToString().Trim();

                string order_no = Request.Form["order_no"].ToString().Trim();
   

				/////////////////////////////   数据签名  /////////////////////////////////
			
                string signStr = "interface_version=" + interface_version + "&merchant_code=" + merchant_code + "&order_no=" + order_no + "&service_type=" + service_type ;

                if (sign_type == "RSA-S") //RSA-S签名方法
                {
                   /**  merchant_private_key，商户私钥，商户按照《密钥对获取工具说明》操作并获取商户私钥。获取商户私钥的同时，也要
						获取商户公钥（merchant_public_key）并且将商户公钥上传到商家后台"公钥管理"（如何获取和上传请看《密钥对获取工具说明》），
						不上传商户公钥会导致调试的时候报错“签名错误”。
				   */
  	
					//demo提供的merchant_private_key是测试商户号100100200200的商户私钥，请自行获取商户私钥并且替换。
                    string merchant_private_key = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAOlji/yfZS3DKefgXJandiPPlheoJeOPgiqdy7WO5kYDxpYIbcmJXp1sIxC0MqY5pyWIOyBlW/NHWDfRyTIYvFm6vxx9m/TeTYKRXHPNdCefXNeV5XoCB7s7gJoeqR1qwgBE0a2Sw4slMrKL4LSv0J0o3po9SgYhP06m3az9dZzDAgMBAAECgYEAgLGdvX1/Y06cyvnS1FgfMvwCvaTAz8yQcEN20vSrUaw/Uvfu4hTXkWLRqyJSU4qy+mU2hVSe8XVD1fPeR7uQD7TWqmXCaIC12DqWFvA2Eio/x4nIixj5t4PN4lbUVBHH/OnHypRWFNcQemR4OWcYCWi5/cYshEaOk9XQnUQKvcECQQD7ZBDA+d8IXM7jtlNNEp+ySw87iz8pKhBoVt3XrAkGczuM9A+ZY+ReViHVxuxqd5AgxSPj1hoQeFR0BnPEVKgzAkEA7ar8kvzp9sPMDujN9QSr+yq73CTlRr8W5q0vMCxsJagie6znvtsAzka3EkI0Srix5fHjRWrWWknblDat5CnpMQJAGfYLhOJJFiP/eaSqlZsGwvLdb9jBfQ7LAvt1jYBGrlPYIoZR1hVq0BM1C06vu479Y2T4f97scib7EcBY1D2p4QJALvdmOEngllI1BhE9ehj1P4o/W46y/FsQ1H8O3y84I+zo+5W8qjuvGhRhYytQGCANj8pQRkyJlYgY614u2PLrUQJAAv9neGYMeFdZnanlsdg6EepWAzvT2tv45WF74BQKJzZF+IFtE1XD85QcqkOJ3o8WgVwXK6/BCn5vNahImz+Ohg==";
                    //私钥转换成C#专用私钥
                    merchant_private_key = testOrderQuery.HttpHelp.RSAPrivateKeyJava2DotNet(merchant_private_key);
                    //签名
                    string signData = testOrderQuery.HttpHelp.RSASign(signStr, merchant_private_key);
                    //将signData进行UrlEncode编码
                    signData = HttpUtility.UrlEncode(signData);
                    //组装字符串
                    string para = signStr + "&sign_type=" + sign_type + "&sign=" + signData;
                    //用HttpPost方式提交
                    string _xml = HttpHelp.httppost("https://query.suifupay.com/query", para, "UTF-8");
                    //将返回的xml中的参数提取出来
                    var el = XElement.Load(new StringReader(_xml));
                    //提取参数
                    var is_success1 = el.XPathSelectElement("/response/is_success");
                    var merchantcode1 = el.XPathSelectElement("/response/trade/merchant_code");
                    var orderno1 = el.XPathSelectElement("/response/trade/order_no");
                    var ordertime1 = el.XPathSelectElement("/response/trade/order_time");
                    var orderamount1 = el.XPathSelectElement("/response/trade/order_amount");
                    var trade_no1 = el.XPathSelectElement("/response/trade/trade_no");
                    var trade_time1 = el.XPathSelectElement("/response/trade/trade_time");
                    var zhfsign1 = el.XPathSelectElement("/response/sign");
                    var trade_status1 = el.XPathSelectElement("/response/trade/trade_status");
                    //去掉首尾的标签并转换成string
                    string is_success = Regex.Match(is_success1.ToString(), "(?<=>).*?(?=<)").Value; //不参与验签
                    if (is_success == "F")
                    {
                        Response.Write("查询失败:" + _xml + "<br/>");
                        Response.End();
                    }
                    string merchantcode = Regex.Match(merchantcode1.ToString(), "(?<=>).*?(?=<)").Value;
                    string orderno = Regex.Match(orderno1.ToString(), "(?<=>).*?(?=<)").Value;
                    string ordertime = Regex.Match(ordertime1.ToString(), "(?<=>).*?(?=<)").Value;
                    string orderamount = Regex.Match(orderamount1.ToString(), "(?<=>).*?(?=<)").Value;
                    string trade_no = Regex.Match(trade_no1.ToString(), "(?<=>).*?(?=<)").Value;
                    string trade_time = Regex.Match(trade_time1.ToString(), "(?<=>).*?(?=<)").Value;
                    string trade_status = Regex.Match(trade_status1.ToString(), "(?<=>).*?(?=<)").Value;
                    string zhfsign = Regex.Match(zhfsign1.ToString(), "(?<=>).*?(?=<)").Value;
                    //组装字符串
                    string signsrc = "merchant_code=" + merchantcode + "&order_amount=" + orderamount + "&order_no=" + orderno + "&order_time=" + ordertime + "&trade_no=" + trade_no + "&trade_status=" + trade_status + "&trade_time=" + trade_time;

                    /**
                     1)zhf_public_key，公钥，每个商家对应一个固定的公钥（不是使用工具生成的密钥merchant_public_key，不要混淆），
                     即为商家后台"公钥管理"->"公钥"里的绿色字符串内容
                     2)demo提供的zhf_public_key是测试商户号100100101102的公钥，请自行复制对应商户号的公钥进行调整和替换。
                     */

                    string zhf_public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCP2DxiRIhY5JCpeQDQO9oJJ5iyZ/HviNzTY23N6XEp+P4tacdftFsgwh1DoR4Qa8MI5Lrmb2WTBlFsybLqM9LYLeBvODokFdAdoi+VOjkESJ4Y8XygiP4Sz7osOwB3MkvLVHAK8e2O8U09Sj2MI034snHvnhCpU3ye3BqkzetLkQIDAQAB";
                    
					//将公钥转换成C#专用格式
                    zhf_public_key = testOrderQuery.HttpHelp.RSAPublicKeyJava2DotNet(zhf_public_key);
                    //验签
                    bool validateResult = testOrderQuery.HttpHelp.ValidateRsaSign(signsrc, zhf_public_key, zhfsign);
                    if (validateResult == false)
                    {
                        Response.Write("验签失败");
                        Response.End();
                    }
                    Response.Write("验签成功");
                }
                else  //RSA签名方法
                {
                    RSAWithHardware rsa = new RSAWithHardware();
                    string merPubKeyDir = "D:/100100101102.pfx";   //证书路径
                    string password = "87654321";                //证书密码
                    RSAWithHardware rsaWithH = new RSAWithHardware();
                    rsaWithH.Init(merPubKeyDir, password, "D:/dinpayRSAKeyVersion");
                    string signData = rsaWithH.Sign(signStr);    //签名
                    signData = HttpUtility.UrlEncode(signData);  //将signData进行UrlEncode编码
                    //组装字符串
                    string para = signStr + "&sign_type=" + sign_type + "&sign=" + signData;
                    //用HttpPost方式提交
                    string _xml = HttpHelp.httppost("https://query.suifupay.com/query", para, "UTF-8");
                    //将返回的xml中的参数提取出来
                    var el = XElement.Load(new StringReader(_xml));
                    //提取参数
                    var is_success1 = el.XPathSelectElement("/response/is_success");
                    var merchantcode1 = el.XPathSelectElement("/response/trade/merchant_code");
                    var orderno1 = el.XPathSelectElement("/response/trade/order_no");
                    var ordertime1 = el.XPathSelectElement("/response/trade/order_time");
                    var orderamount1 = el.XPathSelectElement("/response/trade/order_amount");
                    var trade_no1 = el.XPathSelectElement("/response/trade/trade_no");
                    var trade_time1 = el.XPathSelectElement("/response/trade/trade_time");
                    var zhfsign1 = el.XPathSelectElement("/response/sign");
                    var trade_status1 = el.XPathSelectElement("/response/trade/trade_status");
                    //去掉首尾的标签并转换成string
                    string is_success = Regex.Match(is_success1.ToString(), "(?<=>).*?(?=<)").Value; //不参与验签
                    if (is_success == "F")
                    {
                        Response.Write("查询失败:" + _xml + "<br/>");
                        Response.End();
                    }
                    string merchantcode = Regex.Match(merchantcode1.ToString(), "(?<=>).*?(?=<)").Value;
                    string orderno = Regex.Match(orderno1.ToString(), "(?<=>).*?(?=<)").Value;
                    string ordertime = Regex.Match(ordertime1.ToString(), "(?<=>).*?(?=<)").Value;
                    string orderamount = Regex.Match(orderamount1.ToString(), "(?<=>).*?(?=<)").Value;
                    string trade_no = Regex.Match(trade_no1.ToString(), "(?<=>).*?(?=<)").Value;
                    string trade_time = Regex.Match(trade_time1.ToString(), "(?<=>).*?(?=<)").Value;
                    string trade_status = Regex.Match(trade_status1.ToString(), "(?<=>).*?(?=<)").Value;
                    string zhfsign = Regex.Match(zhfsign1.ToString(), "(?<=>).*?(?=<)").Value;
                    //组装字符串
                    string signsrc = "merchant_code=" + merchantcode + "&order_amount=" + orderamount + "&order_no=" + orderno + "&order_time=" + ordertime + "&trade_no=" + trade_no + "&trade_status=" + trade_status + "&trade_time=" + trade_time;
                    bool result = rsaWithH.VerifySign("100100200200", signsrc, zhfsign);
                    if (result == false)
                    {
                        Response.Write("验签失败");
                        Response.End();
                    }
                    Response.Write("验签成功");
                }


            }
            finally
            {
            }
        }  
    }
}