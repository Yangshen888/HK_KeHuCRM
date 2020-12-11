<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.contactPerson.query.totalCount"
        :pageSize="stores.contactPerson.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.contactPerson.query.kw1"
                      placeholder="输入联系人名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.contactPerson.query.kw2"
                      placeholder="输入客户"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加联系记录"
                >添加联系记录</Button>
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh_Address"></Button>
                </ButtonGroup>
              </Col>
                  <!-- <Button
                  v-can="'shujuqinayi'"
                  icon="md-create"
                  type="primary"
                  @click="shujuqinayianniu"
                  title="数据迁移"
                >数据迁移</Button> -->
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.contactPerson.data"
          :columns="stores.contactPerson.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh_Address"
          :row-class-name="rowClsRender_Address"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{row,index}" slot="state">
            <span>{{CheckState(row.state)}}</span>
          </template>
          <!-- <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" shape="circle" icon="md-trash" v-can="'delete'"></Button>
              </Tooltip>
            </Poptip>
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
          </template>
        </Table>
      </dz-table>
    </Card>

    <Drawer
      title="添加联系记录"
      v-model="formMode2.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formMode2.fields"
        ref="formdispatch"
        :rules="formMode2.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="联系人" prop="contactPersonUuid">
            <Select v-model="formMode2.fields.contactPersonUuid" @on-change="kh" filterable>
              <Option
                v-for="item in this.formMode2.contact"
                :value="item.contactPersonUuid"
                :key="item.contactName"
              >{{item.contactName}}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="客户" prop="clientName">
            <Input v-model="formMode2.fields.clientName" placeholder="请选择联系人" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商机" prop="businessName">
            <Select v-model="formMode2.fields.businessUuid" filterable>
              <Option
                v-for="item in this.formMode2.business"
                :value="item.value"
                :key="item.value"
              >{{item.label}}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系内容" prop="callContent">
            <Input type="textarea" v-model="formMode2.fields.callContent" placeholder="联系内容" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系时间" prop="callTime">
            <!-- <Input type="textarea" v-model="formMode2.fields.callTime" placeholder="联系内容" /> -->
            <Date-picker
              type="datetime"
              @on-change="formMode2.fields.callTime=$event"
              :value="datetimeNow"
              format="yyyy-MM-dd HH:mm"
              placeholder="请选择日期和时间"
              style="width: 200px"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系方式" prop="contactDetailsName" style="width:150px;">
            <Select v-model="formMode2.fields.contactDetailsName">
              <Option
                v-for="item in stores.contactPerson.sources.cityList1"
                :value="item.value"
                :key="item.value"
              >{{item.label}}</Option>
            </Select>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formMode2.opened = false">取 消</Button>
      </div>
    </Drawer>

    <Drawer
      title="修改联系记录"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="false"
    >
      <Form :model="formModel.fields" :rules="formModel.rules" ref="formCarDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="联系人" prop="contactName">
            <Input v-model="formModel.fields.contactName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="客户" prop="clientName">
            <Input v-model="formModel.fields.clientName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商机" prop="businessName">
            <Input v-model="formModel.fields.businessName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系内容" prop="callContent">
            <Input type="textarea" v-model="formModel.fields.callContent" placeholder="联系内容" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系时间" prop="callTime">
            <!-- <Input type="textarea" v-model="formMode2.fields.callTime" placeholder="联系内容" /> -->
            <!-- v-model="formMode2.fields.callTime" -->
            <Date-picker
              type="datetime"
              v-model="formModel.fields.callTime"
              @on-change="formModel.fields.callTime=$event"
              format="yyyy-MM-dd HH:mm"
              placeholder="请选择日期和时间"
              style="width: 200px"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系方式" prop="cdName" style="width:150px;">
            <Select v-model="formModel.fields.cdName">
              <Option
                v-for="item in stores.contactPerson.sources.cityList1"
                :value="item.value"
                :key="item.value"
              >{{item.label}}</Option>
            </Select>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="btnClickEdit">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
// import JSZip from "jszip";
import FileSaver from "file-saver";
import DzTable from "_c/tables/dz-table.vue";
import {
  // sjqianyi,
  lianxiren,
  kehu,
  shangji,
  getAddressInfo,
  create,
  deleteCarDriver,
  editSave, //编辑保存
  loadEditData //加载编辑
} from "@/api/contact/callLog";
import config from "@/config";
export default {
  name: "homeaddress",
  components: {
    DzTable
  },
  data() {
    return {
      commands: {
        export: { name: "export", title: "导出" }
      },

      //list33: [],
      datetimeNow: "",
      //导入
      url: config.baseUrl.dev,
      importdisable: false,

      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },

      // list:[{fileUrl:"",renameFileName:""}],
      // filename:'',

      //form保存参数
      formModel: {
        selection: [],
        opened: false,
        mode: "edit",
        fields: {
          callLogUuid: "",
          clientUuid: "",
          contactPersonUuid: "",
          callContent: "",
          callTime: "",
          callResults: "",
          remarks: "",
          contactName: "",
          clientName: "",
          businessUuid: "",
          contactDetailsUuid: "",
          contactDetailsText: "",
          cdName: "",
          cdText: "",
          contactDetailsName: "",
          userGuid: "",
          userName: ""
        },
        rules: {
          contactName: [
            { type: "string", required: true, message: "请选择联系人" }
          ],
          callContent: [
            { type: "string", required: true, message: "请填写联系内容" }
          ]
          // callTime: [
          //   { type: "string", required: true, message: "请选择联系时间" }
          // ]
        },
        contact: [],
        client: [],
        business: []
      },
      formMode2: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          callLogUuid: "",
          clientUuid: "",
          contactPersonUuid: "",
          callContent: "",
          callTime: "",
          callResults: "",
          remarks: "",
          contactName: "",
          clientName: "",
          businessUuid: "",
          contactDetailsUuid: "",
          contactDetailsText: "",
          contactDetailsName: "",
          userGuid: "",
          userName: ""
        },
        rules: {
          contactPersonUuid: [
            { type: "string", required: true, message: "请选择联系人" }
          ],
          callContent: [
            { type: "string", required: true, message: "请填写联系内容" }
          ]
          // callTime: [
          //   { type: "string", required: true, message: "请选择联系时间" }
          // ]
        },
        contact: [],
        client: [],
        business: []
      },

      //...
      //用于提交及一些保持性数据
      stores: {
        //该实例对象相关数据
        contactPerson: {
          sources: {
            //集合
            clientName: [
              {
                value: "",
                label: ""
              }
            ],
            cityList1: [
              {
                value: "微信",
                label: "微信"
              },
              {
                value: "QQ",
                label: "QQ"
              },
              {
                value: "电话",
                label: "电话"
              },
              {
                value: "面谈",
                label: "面谈"
              },
              {
                value: "邮件",
                label: "邮件"
              }
            ]
          },
          // model1: "",

          //一些自定义附件处理数据,用于下拉列表之类

          //提交请求参数
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDeleted: 0,
            status: -1,
            //自定义参数
            kw: "",
            kw1: "",
            kw2: "",
            //查询排序方式
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          //请求获得的数据集合,用于填入表格
          data: [],
          //一些自定义附件处理数据,用于下拉列表之类

          // url:'',

          //table的列项名称
          columns: [
            //选择框
            // { type: "selection", width: 50, key: "handle" },
            {
              title: "联系人",
              key: "contactName",
              minWidth: 50,
              maxWidth:80,
              sortable: true
            },
            {
              title: "客户",
              key: "clientName",
              minWidth: 80,
              sortable: true
            },
            {
              title: "客户经理",
              key: "clientManager",
              minWidth: 80,
              maxWidth:100,
              sortable: true
            },
            {
              title: "商机",
              key: "businessName",
              minWidth: 80,
              sortable: true
            },
            {
              title: "联系内容",
              key: "callContent",
              minWidth: 600,
              sortable: true
            },
            {
              title: "联系时间",
              key: "callTime",
              minWidth: 80,
              maxWidth:135,
              sortable: true
            },

            {
              title: "联系方式",
              key: "cdName",
              minWidth: 40,
              maxWidth:100,
              sortable: true
            },
            // {
            //   title: "联系号码",
            //   key: "cdText",
            //   minWidth: 40,
            //   sortable: true
            // },
            {
              title: "操作",
              align: "center",
              fixed: "right",
              width: 80,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        },
        xxx2: {}
        //...
      },
      //自定义样式
      styles: {}
    };
  },
  //计算属性,以名称命名的方法体
  computed: {
    formTitle() {
      //条件
      return "";
    },
    //...
    //获取表格选择的行id
    // selectedRows() {
    //   return this.formModel.selection;
    // },
    selectRowsId() {
      //返回具体选择项的uuid
      return this.formModel.selection.map(x => x.homeAddressUuid);
    }
    //...
  },
  //方法集合
  methods: {
    handleToken() {
      btnToken().then(res => {
        console.log(res.data);
      });
    },

    shujuqinayianniu(){
      // sjqianyi().then(res => {
      //   console.log(res)
      // });;
    },

    //加载表格数据
    loadListAddress() {
      this.stores.contactPerson.query.kw = this.$store.state.user.userGuid;
      getAddressInfo(this.stores.contactPerson.query).then(res => {
        console.log(res.data)
        this.stores.contactPerson.data = res.data.data;
        this.stores.contactPerson.query.totalCount = res.data.totalCount;
      });
    },

    //添加按钮（志愿者申请）
    handleShowCreateWindow() {
      //console.log(this.formMode2.fields.callTime + 111 + this.datetimeNow);
      this.loadLianxiren();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      let datetime = new Date();
      this.datetimeNow =
        datetime.getFullYear() +
        "-" +
        (datetime.getMonth() + 1) +
        "-" +
        datetime.getDate() +
        " " +
        datetime.getHours() +
        ":" +
        datetime.getMinutes();
      this.formMode2.fields.callTime = this.datetimeNow;

      //this.formMode2.fields.realName = this.$store.state.user.userName;
    },
    handleSwitchFormModeToCreate() {
      this.formMode2.mode = "create";
    },
    //添加志愿者（保存）
    docreateCarDispatch() {
      this.formMode2.fields.usName = this.$store.state.user.userName;
      this.formMode2.fields.usGuid = this.$store.state.user.userGuid;
      //this.formMode2.fields.callTime = this.datetimeNow;
      //console.log(this.formModel.fields);
      create(this.formMode2.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow(); //关闭表单
          this.loadListAddress();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleSubmitConsumable() {
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formMode2.mode === "create") {
          this.docreateCarDispatch();
        }
      }
    },

    //非空验证提示
    validateDispatchForm() {
      let _valid = false;
      this.$refs["formdispatch"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    validateDispatchForm2() {
      let _valid = false;
      this.$refs["formCarDispatchDetail"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },

    //打开窗口
    handleOpenFormWindow() {
      this.formMode2.opened = true;
    },
    //清空
    handleResetFormDispatch() {
      this.formMode2.fields.businessUuid = null;
      this.formMode2.business = [];
      this.$refs["formdispatch"].resetFields();
    },
    //自动关闭窗口
    handleCloseFormWindow() {
      this.formMode2.opened = false;
    },

    //右边编辑
    handleEdit(row) {
      //console.log(row);
      this.handleOpenFormWindow1();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch1();
      this.doLoadEditData(row.callLogUuid);
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
    },
    doLoadEditData(callLogUuid) {
      loadEditData({ guid: callLogUuid }).then(res => {
        console.log(res);
        //this.stores.contactPerson.sources.clientName = res.data.data.clientName;
        this.formModel.fields = res.data.data[0];
      });
    },

    //编辑商店（保存）
    doEditDispatch() {
      let valid = this.validateDispatchForm2();
      if (!valid) {
        return; 
      }

      this.formModel.fields.usName = this.$store.state.user.userName;
      this.formModel.fields.usGuid = this.$store.state.user.userGuid;
      // console.log(this.formModel.fields);
      editSave(this.formModel.fields).then(res => {
        // console.log(this.formModel.fields)
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow1();
          this.loadListAddress();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    btnClickEdit() {
      this.doEditDispatch();
    },

    //清空
    handleResetFormDispatch1() {
      this.$refs["formCarDispatchDetail"].resetFields();
    },

    //打开窗口
    handleOpenFormWindow1() {
      this.formModel.opened = true;
    },
    //自动关闭窗口
    handleCloseFormWindow1() {
      this.formModel.opened = false;
    },

    handleDelete(row) {
      this.doDelete(row.callLogUuid);
    },
    doDelete(ids) {
      console.log(ids);
      deleteCarDriver({
        ids: ids,
        usName: this.$store.state.user.userName
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadListAddress();
          //this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    //表格行样式
    rowClsRender_Address(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },

    //刷新
    handleRefresh_Address() {
      this.loadListAddress();
    },
    //行选框的改变
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //排序改变(忽略?)
    handleSortChange(column) {
      this.stores.contactPerson.query.sort.direction = column.order;
      this.stores.contactPerson.query.sort.field = column.key;
      this.loadVoteinitiateList();
    },
    //搜索
    handleSearchDispatch() {
      this.loadListAddress();
    },
    //表格翻页
    handlePageChanged(page) {
      this.stores.contactPerson.query.currentPage = page;
      this.loadListAddress();
    },
    //表格页大小改变
    handlePageSizeChanged(pageSize) {
      this.stores.contactPerson.query.pageSize = pageSize;
      this.loadListAddress();
    },

    loadLianxiren() {
      let data = this.$store.state.user.userGuid;
      lianxiren(data).then(res => {
        this.formMode2.contact = res.data.data;
        this.formModel.contact = res.data.data;
      });
    },
    kh(e) {
      if (e != undefined) {
        kehu(e).then(res => {
          this.formMode2.fields.clientName = res.data.data[0].clientName;
          if (res.data.data[0].clientName != null) {
            this.sahngjihuoqu(e);
          }
          //console.log(e);
        });
      }
    },
    kh2(e) {
      if (e != undefined) {
        kehu(e).then(res => {
          this.formModel.fields.clientName = res.data.data[0].clientName;
          //console.log(e);
        });
      }
    },

    sahngjihuoqu(e) {
      let data = e;
      //console.log(e);
      //console.log(data);
      shangji(data).then(res => {
        this.formMode2.business = res.data.data;
      });
    },

    //申请下拉框
    // loadDepartmentList1() {
    //   getkehu().then(res => {
    //     console.log(res.data.data);

    //     this.stores.contactPerson.sources.clientName = res.data.data;
    //     console.log(this.stores.contactPerson.sources.clientName);
    //   });
    // },

    // xz(e){
    //   let college = this.list33.filter(x => x.towns == e);
    //   console.log(college);
    //   console.log(this.formModel3.fields.towns);
    //   console.log(this.formModel3.fields.vname);
    //   this.stores.contactPerson.sources.college=college.map(x => x.vname);
    //   this.loadListAddress();
    // },

    //---------------第二个模型-------------------

    //----------------slot-----------------
    Is_xxx(type) {
      let rType = "未知";
      // switch(type){
      //   case 0:
      //     rType = "否";
      //     break;
      //   case 1:
      //     rType = "是";
      //     break;
      // }
      return rType;
    },
    CheckState(state) {
      if (state == 0) {
        return "否";
      }
      if (state == 1) {
        return "是";
      }
    },
    //------------------杂项---------------------
    //将日期转为YY-MM-DD hh:mm:ss字符串格式
    dateToString(date) {
      let year = date.getFullYear();
      let month = date.getMonth() + 1;
      let day = date.getDate();
      let hour = date.getHours();
      let minute = date.getMinutes();
      let second = date.getSeconds();
      return (
        year +
        "-" +
        month +
        "-" +
        day +
        " " +
        hour +
        ":" +
        minute +
        ":" +
        second
      );
    }
  },

  //页面加载时执行
  mounted() {
    //this.loadDepartmentList1();

    this.loadListAddress();
  }
};
</script>
<style>
.demo-spin-col {
  height: 100px;
  position: absolute;
  left: 35%;
  top: 50%;
}

.demo-spin-icon-load {
  animation: ani-demo-spin 1s linear infinite;
}
</style>
