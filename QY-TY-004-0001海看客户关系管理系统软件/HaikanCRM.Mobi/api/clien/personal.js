import http from '@/components/utils/http.js'


//添加联系人
export const AppAddContact = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppContact/AppAddContact',
		method: 'post',
	}, data)
}

//修改联系人
export const AppEditContact = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppContact/AppEditContact',
		method: 'post',
	}, data)
}


//显示联系人列表
export const GetList = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/Mobi/AppContact/appContectList',
		method: 'post'
	}, data)
}
//通过ID查询联系人详情
export const AppShowContactId = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppContact/AppShowContactId?id=' + data.id,
		method: 'get'
	})
}

// 获取联系人界面联系记录
export const GetContactCallLogList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLogInfo/GetContactCallLogList?contactPersonUuid=' + data.contactPersonUuid,
		method: 'get'
	})
}

// 通过Guid查询联系人详情
export const AppShowContactGuid = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppContact/AppShowContactGuid?contactPersonUuid=' + data.contactPersonUuid,
		method: 'get'
	})
}

//查询出所有状态正常的客户
export const gealluser = (data) => {
	return http.httpRequest({
		url: 'api/v1/contact/contact/GetSelectCustomer?guid='+data,
		method: 'get',
	})
}


