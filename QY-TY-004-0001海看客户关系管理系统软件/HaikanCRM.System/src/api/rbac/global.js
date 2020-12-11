import axios from '@/libs/api.request'

//列表
export const getGlobalList = () => {
    return axios.request({
      url: 'rbac/Global/list',
      method: 'get'
    })
  }
  
  //修改
  export const editGlobalList = (data) => {
    //console.log('传值过去')
    return axios.request({
      url: 'rbac/Global/Edit',
      method: 'post',
      data
    })
}