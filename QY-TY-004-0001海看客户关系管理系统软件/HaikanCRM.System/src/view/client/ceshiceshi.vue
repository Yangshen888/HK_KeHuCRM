<template>
<vxe-table
          resizable
          :tree-config="{children: 'children'}"
          :data="tableData"
          :checkbox-config="{labelField: 'id', highlight: true}"
          @checkbox-change="selectChangeEvent">
          <vxe-table-column type="checkbox" title="ID" width="280" tree-node></vxe-table-column>
          <vxe-table-column field="clientName" title="客户名称"></vxe-table-column>
          <vxe-table-column field="realName" title="客户经理"></vxe-table-column>
          <vxe-table-column field="statusName" title="状态"></vxe-table-column>
          <vxe-table-column field="systemUserName" title="参与人"></vxe-table-column>
		  <vxe-table-column title="操作">
			  <template v-slot="{ row }">
			 <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
			   <Button
			     v-can="'edit'"
			     type="primary"
			     size="small"
			     shape="circle"
			     icon="md-create"
			     @click="handleEdit(row)"
			   ></Button>
			 </Tooltip>
			 <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
			   <Button
			     v-can="'info'"
			     type="success"
			     size="small"
			     shape="circle"
			     icon="md-search"
			     @click="showInfo_xxx(row)"
			   ></Button>
			 </Tooltip>
			  </template>
			  </vxe-table-column>
        </vxe-table>

</template>

	<script src="https://cdn.jsdelivr.net/npm/vue"></script>
	<script src="https://cdn.jsdelivr.net/npm/xe-utils"></script>
	<script src="https://cdn.jsdelivr.net/npm/vxe-table"></script>
<script>
import {
getList
} from "@/api/customer/myClients";
export default {
          data () {
            return {
				  tableData:[]
            }
          },
         created () {
			getList().then(res => {
			  this.tableData = res.data;
			})
          },
          methods: {
            selectChangeEvent ({ records }) {
			  console.info(`勾选${records.length}个树形节点`, records)

			  var cliuuid="";
			  for (let i = 0; i < records.length; i++) {
				   cliuuid+=records[i].clientuuid+",";
			  }
			  console.info(`勾选${records.length}个树形节点uuid`, cliuuid)
			},
			//编辑
			handleEdit(row){
				console.log("点击的行")
				console.log(row)
			},
			//详情
			showInfo_xxx(row){
				console.log("点击的行")
				console.log(row)
			}
          },
		  mounted(){
		  		console.log(this.tableData);	  
		  }
        }

	
</script>

<style>
	@import url("https://cdn.jsdelivr.net/npm/vxe-table/lib/index.css");
</style>
