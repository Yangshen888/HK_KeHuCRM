import axios from '@/libs/api.request'


//  单独行业页面加载
export const loadDataSystemLog = (data) => {
  return axios.request({
    url: 'Customer/Customer/LogList' ,
    method: 'post',
    data
  })
}

