import axios from '@/libs/api.request'

//导入shopInfoImport
export const shopInfoImport = (data) => {
  console.log(data);
  return axios.request({
    url: 'Contact/Contact/shopInfoImport',
    method: 'post',
    data
  })
}

//加载表格数据getAddressInfo
export const getAddressInfo = (data) => {
  return axios.request({
    url: 'Contact/Contact/list',
    method: 'post',
    data
  })
}

// 删除单个
export const deleteCarDriver = (data) => {
  return axios.request({
    url: 'Contact/Contact/delete?ids=' + data.ids + '&usName=' + data.usName,
    method: 'get'
  })
}

//查询出当前商机所属客户的全部联系人
export const jiekoulxr = (data) => {
  return axios.request({
    url: 'Contact/Contact/jiekoulxr?guid=' + data.guid,
    method: 'get'
  })
}


//查询出当前客户所属得全部商机
export const jiekoushangji = (data) => {
  return axios.request({
    url: 'Contact/Contact/jiekoushangji?guid=' + data.guid,
    method: 'get'
  })
}

//查询出当前客户所属得全部商机
export const loadSimpleList = (data) => {
  return axios.request({
    url: 'Contact/Contact/loadSimpleList?guid=' + data.guid,
    method: 'get'
  })
}


//添加联系人信息
export const create = (data) => {
  return axios.request({
    url: 'Contact/Contact/Create',
    method: 'post',
    data
  })
}

//添加联系记录
export const create2 = (data) => {
  return axios.request({
    url: 'Contact/Contact/Create2',
    method: 'post',
    data
  })
}



//获取联系人编辑数据
export const loadEditData = (data) => {
  return axios.request({
    url: 'Contact/Contact/getEdit/' + data.guid,
    method: 'get'
  })
}

//点击客户列表的联系人获取详情
export const loadEditData2 = (data) => {
  return axios.request({
    url: 'Contact/Contact/getEdit2?guid=' + data.guid+"&name="+data.name,
    method: 'get'
  })
}
//点击客户列表的联系人获取详情
export const loadEditDataXQ = (data) => {
  return axios.request({
    url: 'Contact/Contact/loadEditDataXQ?guid=' + data.guid,
    method: 'get'
  })
}




//  编辑（保存）
export const editSave = (data) => {
  return axios.request({
    url: 'Contact/Contact/edit',
    method: 'post',
    data
  })
}


//客户下拉框
export const getkehu = (data) => {
  return axios.request({
    url: 'Contact/Contact/huoqukehu',
    method: 'post',
    data
  })
}


//删除单个联系记录
export const deletelianxijilulog = (data) => {
  return axios.request({
    url: 'Contact/Contact/delete1?ids=' + data.ids + '&usName=' + data.usName,
    method: 'get'
  })
}
//保存编辑经理权限(保存)
export const save_manger=(data)=>{
  return axios.request({
    url: 'Contact/Contact/Save_manger',
    method: 'post',
    data
  })
}

