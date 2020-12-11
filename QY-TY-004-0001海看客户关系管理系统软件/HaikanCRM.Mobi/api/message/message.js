import http from '@/components/utils/http.js'

//显示消息列表
export const MessageShow = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppMessage/MessageShow?guid='+data,
		method: 'get'
	})
}

//删除消息列表
export const MesDelete = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppMessage/MesDelete?guid='+data,
		method: 'get'
	})
}