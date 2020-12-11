import http from '@/components/utils/http.js'

//显示个人信息
export const PersonalList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/DingDing/PersonalList?guid='+data,
		method: 'get'
	})
}

//显示个人信息详情
export const showmantion = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/DingDing/showmantion?guid='+data,
		method: 'get'
	})
}

//编辑个人信息
export const addmantion = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/DingDing/addmantion',
		method: 'post'
	},data)
}