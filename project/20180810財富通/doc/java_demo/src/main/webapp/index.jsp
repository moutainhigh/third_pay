<%@ page language="java" pageEncoding="UTF-8" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
<head>


    <meta http-equiv="pragma" content="no-cache">
    <meta http-equiv="cache-control" content="no-cache">
    <meta http-equiv="expires" content="0">
    <meta http-equiv="keywords" content="keyword1,keyword2,keyword3">
    <meta http-equiv="description" content="This is my page">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no">
    <title>商户支付DEMO - 在线测试</title>
	<link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
	<link href="https://cdn.bootcss.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet">
	<link href="https://cdn.bootcss.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
<style>

body, button, input, select, textarea, h1, h2, h3, h4, h5, h6 {
    font-family: PingFangSC-Regular,'helvetica neue','hiragino sans gb', 'microsoft yahei', tahoma,'microsoft yahei ui', simsun,sans-serif;
}
html, body {

height: 100vh;
}

.small-dialog {
    background: white;
    padding: 0px 30px;
    text-align: left;
    max-width: 600px;
    margin: 0px auto;
    position: relative;
}
.btn {
	display: inline-block;	
    font-weight: 400;
    white-space: nowrap;
    vertical-align: middle;
    cursor: pointer;
    user-select: none;
    background-image: none;
    border: 1px solid transparent;
    padding: 9px 12px;
    font-size: 14px;
    line-height: 1.42857143;
    min-width: 120px;
    text-align: center;
    border-radius: 0;
    text-transform: uppercase;

    -webkit-transition: all 0.2s ease-in-out;
    -moz-transition: all 0.2s ease-in-out;
    -ms-transition: all 0.2s ease-in-out;
    -o-transition: all 0.2s ease-in-out;
    transition: all 0.2s ease-in-out;
	/*margin-right: 20px;*/
}

.form-control {
    display: block;
    width: 100%;
    padding: 6px 12px;
    font-size: 14px;
    line-height: 1.42857143;
    border: 1px solid #ccc;
    transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
    border-radius: 0;
    background: #ffffff;
    height: 40px;
    padding-right: 30px;
    border-color: #acacac;
    color: #666666;
    position: relative;
    -webkit-appearance: none;
    -webkit-box-shadow: inset 0 1px 2px rgba(0, 0, 0, .075);
    box-shadow: inset 0 1px 2px rgba(0, 0, 0, .075);
}

</style>	
</head>
<body>
	
	<div class="mfp-content">
		<div id="moneyin-dialog" class="zoom-anim-dialog small-dialog">
			<div class="form-group">
				<div class="checkbox">
					<label><input type="checkbox" name="checkbox" value="1" id="checkbox">是否填写商户信息</label>
				</div>	
			</div>			
			
			<div class="form-group" id="user_key" style="display: none;">
				<h3>商户Api配置</h3>
				<p><input type="text" class="form-control" id="apicode" value="33396133" placeholder="此处填写商户号"></p>
				<p><input type="text" class="form-control" id="apikey" value="879adf264bf0fc038d427a413ede6b03" placeholder="此处填写商户密钥"></p>
			</div>				
		</div>	
	</div>

	<div class="mfp-content">
		<div id="moneyin-dialog" class="zoom-anim-dialog small-dialog">		  
		  <div class="form-group">
			<h3>支付金额 ￥</h3>
			<p><input type="number" class="form-control"  style="width:50%" id="inputprice" value="1.00" placeholder="请输入金额，例如：50.00"></div></p>
			<p>
				<div class="radio">				
					<label><input type="radio" name="return_type" value="data" checked="">默认</label>&nbsp;&nbsp;
					<label><input type="radio" name="return_type" value="json">json</label>				
				</div>					
			</p>
			<p>
				<select name="istype" class="form-control">
					<!--option value="">选择支付类型</option-->
					<option value="wechat">微信</option>					
					<option value="alipay">支付宝</option>
					<option value="wechat1">微信1</option>
					<option value="alipay1">支付宝1</option>
					<option value="wechat2">微信2</option>
					<option value="alipay2">支付宝2</option>
					<option value="wechat3">微信3</option>
					<option value="alipay3">支付宝3</option>
                </select>
			</p>
			<p>
				<a href="#" id="AJAX" class="btn btn-success">发起支付</a>	

			</p>
		</div>
	</div>


    <form style='display:none;' id='formpay' name='formpay' method='post' action='http://47.75.202.59:10001/channel/Common/mail_interface'>
		<input name='return_type' id='return_type' type='text' value=''/> 
		<input name='api_code' id='api_code' type='text' value=''/>        
        <input name='is_type' id='is_type' type='text' value='' />        
        <input name='price' id='price' type='text' value=''/>
        <input name='order_id' id='order_id' type='text' value=''/>
		<input name='time' id='time' type='text' value=''/>
        <input name='mark' id='mark' type='text' value=''/>		      
        <input name='return_url' id='return_url' type='text' value=''/>
		<input name='notify_url' id='notify_url' type='text' value=''/>
        <input name='sign' id='sign' type='text' value=''/>
        <input type='submit' id='submitdemo'>
    </form>

<!-- Jquery files -->
<script type="text/javascript" src="https://cdn.staticfile.org/jquery/1.11.1/jquery.min.js"></script>
<script type="text/javascript">
$().ready(function(){

	$("#checkbox").click(function(){
        if($(this).is(':checked')){
			$("#user_key").toggle();			
		}else{
			$("#user_key").toggle();
		}
    });	
	
    function get_typejson(){
        return ($('input[name="return_type"]:checked').val()); 
    }	
	
    function get_istype(){
        return ($('select[name="istype"] option:checked').val()); 
    }	
	
    $("#AJAX").click(function(){
        $.ajax({
            url: "pay.do",
            type: "post",
            dataType: "json",
            contentType: "application/json;charset=utf-8",//发送类型
            data: JSON.stringify({

                "price" : $("#inputprice").val(),
                "api_code" : $("#apicode").val(),
                "api_key" : $("#apikey").val(),
                "return_type" : get_typejson(),
                "is_type" : get_istype()
            }),
            success: function (data) {
                   console.log(data);
					$('#return_type').val(data.return_type);
					$('#api_code').val(data.api_code);
                    $("#is_type").val(data.is_type);
                    $('#price').val(data.price);
                    $('#order_id').val(data.order_id);
                    $('#mark').val(data.mark);
                    $('#time').val(data.time);
                    $('#return_url').val(data.return_url);
					$('#notify_url').val(data.notify_url);
                    $('#sign').val(data.sign);
                    $('#submitdemo').click();									

			},
			error:function(error){
				alert('error');
			}
		});	
    });
	
});
</script>    


</body>
</html>