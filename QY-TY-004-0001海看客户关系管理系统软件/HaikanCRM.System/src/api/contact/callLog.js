import axios from '@/libs/api.request'




//参与人穿梭框
export const sjqianyi = (data) => {
  return axios.request({
    url: 'Contact/CallLog/sjqianyi',
    method: 'get',
  })
}


//加载表格数据getAddressInfo
export const getAddressInfo = (data) => {
  return axios.request({
    url: 'Contact/Contact/list1',
    method: 'post',
    data
  })
}


  // 删除单个联系记录
  export const deleteCarDriver = (data) => {
    return axios.request({
      url: 'Contact/Contact/delete1?ids=' + data.ids + '&usName=' + data.usName,
      method: 'get'
    })
  }

  

//添加地址
export const create = (data) => {
  return axios.request({
    url: 'Contact/Contact/Create1',
    method: 'post',
    data
  })
}


//  编辑加载
export const loadEditData = (data) => {
  return axios.request({
    url: 'Contact/Contact/getEdit1/' + data.guid,
    method: 'get'
  })
}



//  编辑（保存）
export const editSave = (data) => {
  return axios.request({
    url: 'Contact/Contact/edit1',
    method: 'post',
    data
  })
}

//联系人下拉框
export const lianxiren = (data) => {
  return axios.request({
    url: 'Contact/CallLog/lianxiren?guid='+data,
    method: 'get'
  })
}


//通过联系人获取客户
export const kehu = (data) => {
  return axios.request({
    url: 'Contact/CallLog/kehu?guid=' + data,
    method: 'get'
  })
}

//通过联系人获取商机
export const shangji = (data) => {
  return axios.request({
    url: 'Contact/Contact/shangji?shangji=' + data,
    method: 'get'
  })
}

// //客户下拉框
// export const getkehu = (data) => {
//   return axios.request({
//     url: 'Contact/Contact/huoqukehu',
//     method: 'post',
//     data
//   })
// }
