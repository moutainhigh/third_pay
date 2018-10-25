<?php
/**
 * 客户端请求本接口 实现支付
 * author: fengxie
 * Date: 2016/10/7
 */
include('./config.php');
$orderId = time() . mt_rand(100, 9999); //商户订单号

$data = array(
    "fx_merchant_id" => $fxid, //商户号
    "fx_order_id" => $orderId, //商户订单号
    "fx_order_amount" => 1000, //支付金额 单位元
    "fx_notify_url" => $notifyUrl, //异步回调 , 支付结果以异步为准 不带任何参数否则通知失败
    "fx_back_url" => $backUrl, //同步回调 不作为最终支付结果为准，请以异步回调为准 不带任何参数
    "fx_pay" => "zfbsm", //支付宝zfbsm 微信wxsm QQ钱包 qqsm
//    "fx_desc" => "test", //商品名                可不传
//    "fx_attch" => 'my fxattch', //附加信息        可不传
//    "fx_ip" => '123.123.123.123', //支付端ip地址  可不传
//    'fx_return_url' => 100 //返回json，其中fx_cashier_url为我方收银台地址，可直接响应给用户做get跳转，或者使用此url的qr_code_url，qr_code_url为官方二维码链接地址！
);


$data["fx_sign"] = md5(md5($data["fx_merchant_id"] . '|' .  $data["fx_order_id"] . '|'  . $data["fx_order_amount"] . '|'  . $data["fx_notify_url"] . '|'  . $fxkey));

//组装form表单提交
$str = '<form id="Form" name="Form" method="post" action="' . $tjurl . '">';
foreach ($data as $key => $val) {
    if (empty($val))
        continue;
    $str = $str . '<input type="hidden" name="' . $key . '" value="' . $val . '"/>';
}
$str = $str . '</form>';
$str = $str . '<script>';
$str = $str . 'document.Form.submit();';
$str = $str . '</script>';
die($str);

?>