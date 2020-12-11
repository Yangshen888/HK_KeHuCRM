import http from '@/components/utils/http.js'
//获取全部联系记录
export const GetPogodaList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Mobi/AppCallLog/AppCallLogList',
		method: 'post'
	}, data)
}
