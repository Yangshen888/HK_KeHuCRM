import http from '@/components/utils/http.js'

//显示商机列表
export const BusShow = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/business/business/BusShow',
		method: 'post'
	}, data)
}

//添加商机or编辑商机
export const BusAdd = (data) => {
	return http.httpRequest({
		url: 'api/v1/business/business/BusAdd',
		method: 'post'
	}, data)
}

//查询出所有状态正常的客户
export const GetBusSelectCus = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/GetSelectCustomer?guid='+data,
		method: 'get',
	})
}

//显示商机详情
export const BusView = (data) => {
	return http.httpRequest({
		url: 'api/v1/business/business/BusView?guid='+data,
		method: 'get'
	})
}