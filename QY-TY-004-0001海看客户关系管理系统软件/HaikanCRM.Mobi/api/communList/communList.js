import http from '@/components/utils/http.js'

//显示通讯录
export const UserList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppMessage/UserList?name='+data,
		method: 'get'
	})
}