import axios from '@/libs/api.request'


//首页基础数据统计
export const GetBaseInfo = (data) => {
  return axios.request({
    url: 'IndexStatistics/IndexStatistics/GetBaseInfo',
    method: 'get',
  })
}

//首页统计客户地区饼图
export const GetCircle = (data) => {
  return axios.request({
    url: 'IndexStatistics/IndexStatistics/GetCircle',
    method: 'get',
  })
}

//首页柱形统计图
export const GetBarChart = (data) => {
  return axios.request({
    url: 'IndexStatistics/IndexStatistics/GetBarChart',
    method: 'get',
  })
}

//首也折线统计图
export const GetLineChart = (data) => {
  return axios.request({
    url: 'IndexStatistics/IndexStatistics/GetLineChart',
    method: 'get',
  })
}
