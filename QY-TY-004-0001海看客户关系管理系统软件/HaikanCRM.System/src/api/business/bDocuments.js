import axios from '@/libs/api.request'

//商机文件列表
export const getFileList = (data) => {
  return axios.request({
    url: 'business/BusinessDocuments/list',
    method: 'post',
    data
  })
}



// 添加商机文件
export const createFile = (data) => {
  return axios.request({
    url: 'business/BusinessDocuments/create',
    method: 'post',
    data
  })
}

//获取编辑的商机文件信息
export const loadFile = (data) => {
  return axios.request({
    url: 'business/BusinessDocuments/edit/' + data.guid,
    method: 'get'
  })
}

// 保存编辑的商机文件信息
export const editFile = (data) => {
  return axios.request({
    url: 'business/BusinessDocuments/edit',
    method: 'post',
    data
  })
}

// 删除文件
export const deleteFile = (ids) => {
  return axios.request({
    url: 'business/BusinessDocuments/delete/' + ids,
    method: 'get'
  })
}

// 批量删除文件
export const batchCommandFile = (data) => {
  return axios.request({
    url: 'business/BusinessDocuments/batch',
    method: 'get',
    params: data
  })
}


