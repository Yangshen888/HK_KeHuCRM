import axios from '@/libs/api.request'


//  单独行业页面加载
export const loadDataIndusy = (data) => {
  return axios.request({
    url: 'Customer/Customer/ClientIndustryList' ,
    method: 'post',
    data
  })
}

//修改行业信息
export const IdunesyEdit = (data) => {
	return axios.request({
    url: 'Customer/Customer/IdunesyEdit',
    method: 'post',
    data
	})
}
//添加行业信息
export const IdunesyAdd = (data) => {
	return axios.request({
    url: 'Customer/Customer/IdunesyAdd',
    method: 'post',
    data
	})
}



//显示行业详情
export const IdunesyView = (data) => {
	return axios.request({
		url: 'Customer/Customer/IdunesyView?guid='+data,
		method: 'get'
	})
}


//删除行业信息
export const Batch = (data) => {
	return axios.request({
    url: 'Customer/Customer/IndustryBatch',
    method: 'get',
    params:data
	})
}


