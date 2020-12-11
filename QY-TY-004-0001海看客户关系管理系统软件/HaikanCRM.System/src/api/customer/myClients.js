import axios from '@/libs/api.request'

//客户列表
export const getClientsList = (data) => {
  return axios.request({
    url: 'Customer/Customer/list',
    method: 'post',
    data
  })
}

//树形控件 客户列表
export const getList = (data) => {
  return axios.request({
    url: 'Customer/Customer/getList',
    method: 'post',
    data
  })
}

//  编辑加载
export const loadEditData = (data) => {
  return axios.request({
    url: 'Customer/Customer/getEdit/' + data.guid,
    method: 'get'
  })
}

//参与人穿梭框
export const getcanyuren = (data) => {
  return axios.request({
    url: 'Customer/Customer/allsystemuser',
    method: 'get',
  })
}
//获取已选择的人员
export const binddataok = (data) => {
  return axios.request({
    url: 'Customer/Customer/selectsystemuser/'+data.id,
    method: 'get',
    params: data
  })
}
//获取已选择的人员
export const binddataok1 = (data) => {
  return axios.request({
    url: 'Customer/Customer/selectsystemuser1/'+data.id,
    method: 'get',
    params: data
  })
}

//  商机加载
export const loadEditData1 = (data) => {
  return axios.request({
    url: 'Customer/Customer/getEdit1' ,
    method: 'post',
    data
  })
}

//  单独商机页面加载
export const loadEditData1sahngji = (data) => {
  return axios.request({
    url: 'business/Business/GetEdit1shangji' ,
    method: 'post',
    data
  })
}
//显示商机详情
export const BusView = (data) => {
	return axios.request({
		url: 'business/business/BusView?guid='+data,
		method: 'get'
	})
}

//显示商机详情
export const BusView2 = (data) => {
	return axios.request({
		url: 'Customer/Customer/BusView2',
    method: 'post',
    data
	})
}

//显示商机详情
export const BusViewlxrLog = (data) => {
	return axios.request({
		url: 'Customer/Customer/BusViewlxrLog',
    method: 'post',
    data
	})
}

//显示当前商机信息
export const BusView3 = (data) => {
	return axios.request({
		url: 'Customer/Customer/BusView3?guid='+data,
		method: 'get'
	})
}

//查询出所有状态正常的客户
export const gealluser = (data) => {
	return axios.request({
		url: 'Contact/Contact/GetSelectCustomer/',
		method: 'get',
	})
}

//查询出所有客户的下拉列表
export const GetAllCustomer = (data) => {
	return axios.request({
		url: 'Contact/Contact/GetAllCustomer/',
		method: 'get',
	})
}



//查询出所有状态正常（IsDelete == 0）的客户经理
export const geallmanager = (data) => {
	return axios.request({
		url: 'Contact/Contact/Geallmanager/',
		method: 'get',
	})
}


//修改商机信息
export const BusEdit = (data) => {
	return axios.request({
    url: 'Customer/Customer/BusEdit',
    method: 'post',
    data
	})
}
//添加商机信息
export const BusAdd = (data) => {
	return axios.request({
    url: 'Customer/Customer/BusAdd',
    method: 'post',
    data
	})
}

//删除商机信息
export const Batch = (data) => {
	return axios.request({
    url: 'Customer/Customer/Batch',
    method: 'get',
    params:data
	})
}

//  加载联系人信息
export const getTravel = (data) => {
  return axios.request({
    url: 'Customer/Customer/getTravel',
    method: 'post',
    data
  })
}


//  编辑（保存）
export const editSave = (data) => {
  return axios.request({
    url: 'Customer/Customer/edit',
    method: 'post',
    data
  })
}

//添加联系人
export const EditContactPerson = (data) => {
  return axios.request({
    url: 'Customer/Customer/EditContactPerson',
    method: 'post',
    data
  })
}
  // 删除单个客户
  export const deleteCarDriver = (data) => {
    return axios.request({
      url: 'Customer/Customer/delete?ids=' + data.ids + '&usName=' + data.usName,
      method: 'get'
    })
  }


//  商机（保存）
export const editSaveBusi = (data) => {
  return axios.request({
    url: 'Customer/Customer/EditBusiness',
    method: 'post',
    data
  })
}
//添加
export const create1 = (data) => {
  return axios.request({
    url: 'Customer/Customer/Create',
    method: 'post',
    data
  })
}
//获取全部状态
export const getAllStatus = () => {
  return axios.request({
    url: 'Customer/Customer/AllStatus',
    method: 'get',

  })
}
//获取全部客户类型
export const getAllTypes = () => {
  return axios.request({
    url: 'Customer/Customer/AllTypes',
    method: 'get',
  })
}

//获取全部行业
export const getAllIndustry = () => {
  return axios.request({
    url: 'Customer/Customer/AllIndustry',
    method: 'get',
  })
}

//load menu truee
export const loadMenuTree = () => {
  return axios.request({
    url: "Customer/Customer/tree",
    method: "get"
  });
};

//load menu truee
export const SuploadMenuTree = (data) => {
  return axios.request({
    url: "Customer/Customer/suptree?guid="+data,
    method: "get"
  });
};