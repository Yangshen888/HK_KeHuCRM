import http from '@/components/utils/http.js'


// 添加客户
export const AppCustomerAdd = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/AppCustomerAdd',
		method: 'post',
	}, data)
}
//获取客户列表
export const GetPogodaList = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/Mobi/AppCustomer/AppCustomerList',
		method: 'post'
	}, data)
}
//获取客户详情
export const GetPogodaID = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/AppCustomerInfo?clientUuid=' + data.clientUuid,
		method: 'get'
	})
}
//获取该客户全部联系记录		
export const GetCallLogList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/AppCustomerCallLog?clientUuid=' + data.clientUuid,
		method: 'get'
	})
}


//获取所有用户
export const GelUser = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/GeallUser/',
		method: 'get',
	})
}


//当前客户的所有相关联系人
export const AppbusRelatedConName = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/AppbusRelatedConName',
		method: 'post'
	},data)
}
//获取所选客户相关联系人
export const AppbusRelatedConGuid = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/AppbusRelatedConGuid',
		method: 'post'
	},data)
}

//查询全部客户赋值下拉框
export const GetCustomerDropdown = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/GetCustomerDropdown/',
		method: 'get',
	})
}

//查询全部行业赋值下拉框
export const GetCustomerIndustry = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/GetCustomerIndustry/',
		method: 'get',
	})
}
//获取全部状态
export const AllStatus = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/AllStatus/',
		method: 'get',
	})
}
//获取全部类型
export const AllTypes = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/AllTypes/',
		method: 'get',
	})
}
//获取全部行业
export const AllIndustry = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/AllIndustry/',
		method: 'get',
	})
}
// 根据uuid查询客户信息
export const AppGetCustomerInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/AppGetCustomerInfo?id=' + data.id,
		method: 'get'
	})
}

// 保存编辑后客户信息
export const SavaEditCustomerInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/SavaEditCustomerInfo',
		method: 'post',
	}, data)
}


// 查询客户赋值模糊匹配控件
export const VagueSelectCustomer = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/Mobi/AppCustomer/VagueSelectCustomer/',
		method: 'get',
	})
}
// 查询联系人赋值模糊匹配控件
export const VagueSelectContact = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/VagueSelectContact/',
		method: 'get',
	})
}
// 查询商机赋值模糊匹配控件
export const VagueSelectBusiness = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCustomer/VagueSelectBusiness/',
		method: 'get',
	})
}