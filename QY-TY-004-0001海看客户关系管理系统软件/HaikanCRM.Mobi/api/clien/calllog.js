import http from '@/components/utils/http.js'

//获取联系记录列表
export const GetPogodaList = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/Mobi/AppCallLog/AppCallLogList',
		method: 'post'
	}, data)
}

//获取此客户全部联系记录
export const GetCustomerCallLog = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/CallLogList?clientUuid=' + data.clientUuid,
		method: 'get'
	})
}

//联系记录详情
export const AppCallLogInfo = (data) => {

	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/AppCallLogInfo/' + data.callLogUuid,
		method: 'get'
	})
}
//根据客户的uuid获取联系人下拉框的值
export const gealluser = (data) => {
	return http.httpRequest({
		url: 'api/v1/Contact/callLog/gealluser?clientUuid=' + data.clientUuid,
		method: 'get'
	})
}
//根据客户ID获取商机下拉框的值
export const GetCusIdSelectBus = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/GetCusIdSelectBus?clientUuid=' + data.clientUuid+'&guid='+data.guid,
		method: 'get'
	})
}
//根据输入客户获取该客户联系人
export const PassCusSelectCon = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/PassCusSelectCon' ,
		method: 'post'
	},data)
}
//根据输入客户获取该客户商机
export const PassCusSelectBus = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/PassCusSelectBus' ,
		method: 'post'
	},data)
}

//修改客户联系记录  AppEditCusCallLog
export const AppEditCallLog = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/AppEditCallLog',
		method: 'post'
	}, data)
}
//添加联系记录
export const AppAddCallLog = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/AppAddCallLog',
		method: 'post'
	}, data)
}
//联系人详情页面跳转添加联系记录
export const AppConSkipAddCallLog = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/AppConSkipAddCallLog',
		method: 'post'
	}, data)
}

// 查询出所有客户
export const GetSelectCustomer = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/GetSelectCustomer?guid='+data,
		method: 'get',
	})
}
//钉钉免登
export const getuserinfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/DingDing/getuserinfo/' + data,
		method: 'Get'
	})
}
//通过钉钉ID查询该用户信息
export const GetSystemUser = (data) => {
	return http.httpRequest({
		url: 'api/v1/Contact/callLog/GetSystemUser?userid=' + data,
		method: 'get'
	})
}