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
                      style="width: 150px"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.contactPerson.query.kw"
                      placeholder="输入联系人名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                    <Input
                      style="width: 150px"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.contactPerson.query.kw1"
                      placeholder="输入所属客户名称"
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
                  title="添加联系人"
                  >添加联系人</Button
                >
                <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportUserInfo"
                  title="导入联系人信息"
                  >导入联系人信息</Button
                >
                <ButtonGroup class="mr3">
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh_Address"
                  ></Button>
                </ButtonGroup>
              </Col>
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
          <template slot-scope="{ row, index }" slot="state">
            <span>{{ CheckState(row.state) }}</span>
          </template>
          <!-- <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  type="error"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                  v-can="'delete'"
                ></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="联系记录"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'buslog'"
                type="warning"
                size="small"
                shape="circle"
                icon="ios-cloud-upload"
                @click="bushandleLog(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
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
        </Table>
      </dz-table>
    </Card>
    <Drawer
      title="联系人详情"
      v-model="formModeCP.opened"
      width="550"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModeCP.fields" ref="fromCP" label-position="top">
        <Row :gutter="16">
          <FormItem label="联系人姓名" prop="contactName">
            <Input
              readonly
              v-model="formModeCP.fields.contactName"
              placeholder="联系人姓名"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="所属客户" prop="clientUuid">
            <Select
              disabled
              v-model="formModeCP.fields.clientUuid"
              style="width: 400px"
              filterable
            >
              <Option
                v-for="item in stores.contactPerson.sources.clientName"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16" style="margin-left: -16px">
          <Col span="12">
            <FormItem label="职务" prop="call">
              <Input
                readonly
                v-model="formModeCP.fields.call"
                placeholder="职务"
              />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="性别" prop="sex">
              <Select
                disabled
                v-model="formModeCP.fields.sex"
                style="width: 150px"
                placeholder="性别"
              >
                <Option
                  v-for="item in stores.contactPerson.cityList"
                  :value="item.value"
                  :key="item.value"
                  >{{ item.label }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="生日" prop="dateBirth">
            <DatePicker
              disabled
              v-model="formModeCP.fields.dateBirth"
              @on-change="formModeCP.fields.dateBirth = $event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="生日"
              style="width: 200px"
              :options="options3"
            ></DatePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="电话(如有多个以空格隔开)" prop="phone">
            <Input
              readonly
              v-model="formModeCP.fields.phone"
              placeholder="电话号码"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="邮箱" prop="email">
            <Input
              readonly
              v-model="formModeCP.fields.email"
              placeholder="邮箱"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="微信" prop="weChat">
            <Input
              readonly
              v-model="formModeCP.fields.weChat"
              placeholder="微信"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="QQ" prop="tencentQq">
            <Input
              readonly
              v-model="formModeCP.fields.tencentQq"
              placeholder="QQ"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="家庭地址" prop="familyAddress">
            <Input
              readonly
              v-model="formModeCP.fields.familyAddress"
              placeholder="家庭地址"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="办公地址" prop="officeAddress">
            <Input
              readonly
              v-model="formModeCP.fields.officeAddress"
              placeholder="办公地址"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input
              readonly
              type="textarea"
              v-model="formModeCP.fields.remarks"
              placeholder="备注信息"
            />
          </FormItem>
        </Row>
      </Form>
    </Drawer>

    <Drawer
      title="联系人信息导入"
      v-model="formimport.opened"
      width="600"
      :mask-closable="true"
      :mask="true"
    >
      <div>
        <input
          ref="inputer"
          id="upload"
          type="file"
          @change="importfxx"
          accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        />
        <Button
          v-can="'model'"
          icon="ios-cloud-download"
          type="warning"
          @click="handleimportmodel"
          title="联系人信息导入模板"
          >联系人信息导入模板</Button
        >
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
          >导入</Button
        >

        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>

    <Drawer
      title="添加联系人"
      v-model="formMode2.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formMode2.fields"
        ref="formdispatccc2"
        :rules="formMode2.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="客户经理">
            <Input v-model="formMode2.fields.userName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系人姓名" prop="contactName">
            <Input
              v-model="formMode2.fields.contactName"
              placeholder="联系人姓名"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="所属客户" prop="clientUuid">
            <Select
              v-model="formMode2.fields.clientUuid"
              style="width: 400px"
              filterable
            >
              <Option
                v-for="item in stores.contactPerson.sources.clientName"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16" style="margin-left: -16px">
          <Col span="12">
            <FormItem label="职务" prop="call">
              <Input v-model="formMode2.fields.call" placeholder="职务" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="性别" prop="sex">
              <Select
                v-model="formMode2.fields.sex"
                style="width: 150px"
                placeholder="性别"
              >
                <Option
                  v-for="item in stores.contactPerson.cityList"
                  :value="item.value"
                  :key="item.value"
                  >{{ item.label }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="生日" prop="dateBirth">
            <DatePicker
              v-model="formMode2.fields.dateBirth"
              @on-change="formMode2.fields.dateBirth = $event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="生日"
              style="width: 200px"
              :options="options3"
            ></DatePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="电话(如有多个以空格隔开)" prop="phone">
            <Input v-model="formMode2.fields.phone" placeholder="电话号码" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="邮箱" prop="email">
            <Input v-model="formMode2.fields.email" placeholder="邮箱" />
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="性别" prop="sex">
            <Select v-model="formMode2.fields.sex" style="width:150px" placeholder="性别">
              <Option
                v-for="item in stores.contactPerson.cityList"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
          </FormItem>
        </Row>-->
        <Row :gutter="16">
          <FormItem label="微信" prop="weChat">
            <Input v-model="formMode2.fields.weChat" placeholder="微信" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="QQ" prop="tencentQq">
            <Input v-model="formMode2.fields.tencentQq" placeholder="QQ" />
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="手机号码" prop="bgPhone">
            <Input v-model="formMode2.fields.bgPhone" placeholder="请输入手机号码" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="座机号码" prop="zjPhone">
            <Input v-model="formMode2.fields.zjPhone" placeholder="请输入座机号码" />
          </FormItem>
        </Row>-->
        <Row :gutter="16">
          <FormItem label="家庭地址" prop="familyAddress">
            <Input
              v-model="formMode2.fields.familyAddress"
              placeholder="家庭地址"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="办公地址" prop="officeAddress">
            <Input
              v-model="formMode2.fields.officeAddress"
              placeholder="办公地址"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input
              type="textarea"
              v-model="formMode2.fields.remarks"
              placeholder="备注信息"
            />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formMode2.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>

    <Drawer
      title="修改联系人信息"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModel.fields"
        ref="formCarDispatchDetail"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="联系人姓名" prop="contactName">
            <Input
              v-model="formModel.fields.contactName"
              placeholder="联系人姓名"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="所属客户" prop="clientUuid">
            <Select
              v-model="formModel.fields.clientUuid"
              style="width: 400px"
              :disabled="true"
            >
              <Option
                v-for="item in stores.contactPerson.sources.clientName"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16" style="margin-left: -16px">
          <Col span="12">
            <FormItem label="职务">
              <Input v-model="formModel.fields.call" placeholder="职务" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="性别">
              <Select
                v-model="formModel.fields.sex"
                style="width: 150px"
                placeholder="性别"
              >
                <Option
                  v-for="item in stores.contactPerson.cityList"
                  :value="item.value"
                  :key="item.value"
                  >{{ item.label }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="生日" prop="dateBirth">
            <DatePicker
              v-model="formModel.fields.dateBirth"
              @on-change="formModel.fields.dateBirth = $event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="生日"
              style="width: 200px"
              :options="options3"
            ></DatePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="电话(如有多个以空格隔开)" prop="phone">
            <Input v-model="formModel.fields.phone" placeholder="电话号码" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="邮箱" prop="email">
            <Input v-model="formModel.fields.email" placeholder="邮箱" />
          </FormItem>
        </Row>
        <Row :gutter="16"></Row>
        <Row :gutter="16">
          <FormItem label="微信" prop="weChat">
            <Input v-model="formModel.fields.weChat" placeholder="微信" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="QQ" prop="tencentQq">
            <Input v-model="formModel.fields.tencentQq" placeholder="QQ" />
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="手机号码" prop="bgPhone">
            <Input v-model="formModel.fields.bgPhone" placeholder="请输入手机号码" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="座机号码" prop="zjPhone">
            <Input v-model="formModel.fields.zjPhone" placeholder="请输入座机号码" />
          </FormItem>
        </Row>-->
        <Row :gutter="16">
          <FormItem label="家庭地址" prop="familyAddress">
            <Input
              v-model="formModel.fields.familyAddress"
              placeholder="请输入家庭地址"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="办公地址" prop="officeAddress">
            <Input
              v-model="formModel.fields.officeAddress"
              placeholder="请输入办公地址"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input
              type="textarea"
              v-model="formModel.fields.remarks"
              placeholder="备注信息"
            />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="btnClickEdit"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>

    <Drawer
      title="联系记录列表"
      v-model="formModelLog.opened"
      width="800"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModelLog.fields"
        ref="fromcreat12"
        :rules="formModelLog.rules"
        label-position="top"
      >
        <dz-table
          class="carlist"
          :totalCount="formModelLog.buslist.query.totalCount"
          :pageSize="formModelLog.buslist.query.pageSize"
          @on-page-change="handlePageChanged4"
          @on-page-size-change="handlePageSizeChanged4"
        >
          <div slot="searcher" style="text-align: right">
            <section class="dnc-toolbar-wrap">
              <Row :gutter="16">
                <Button
                  v-can="'buscreate'"
                  icon="md-create"
                  type="primary"
                  @click="handleCreatelianxirenLog"
                  title="添加联系记录"
                  >添加联系记录</Button
                >
              </Row>
            </section>
          </div>
          <Table
            slot="table"
            ref="tables"
            :border="false"
            size="small"
            :highlight-row="true"
            @on-select="BushandleSelect1"
            @on-selection-change="BushandleSelectionChange1"
            :data="formModelLog.buslist.data"
            :columns="formModelLog.buslist.columns"
            :row-class-name="rowClsRender_xxx"
          >
            <template slot-scope="{ row, index }" slot="state">
              <span>{{ CheckState(row.state) }}</span>
            </template>
            <template slot-scope="{ row, index }" slot="action">
              <Poptip
                confirm
                :transfer="true"
                title="确定要删除吗?"
                @on-ok="lianxijiluDelete(row)"
              >
                <Tooltip
                  placement="top"
                  content="删除"
                  :delay="1000"
                  :transfer="true"
                >
                  <Button
                    type="error"
                    size="small"
                    shape="circle"
                    icon="md-trash"
                    v-can="'deleteLog'"
                  ></Button>
                </Tooltip>
              </Poptip>
              <!-- <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
                      <Button
                        v-can="'busedit'"
                        type="primary"
                        size="small"
                        shape="circle"
                        icon="md-create"
                        @click="bushandleEdit(row)"
                      ></Button>
              </Tooltip>-->
            </template>
          </Table>
        </dz-table>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 8px" icon="md-close" @click="doCancel_xxx"
          >取 消</Button
        >
      </div>
    </Drawer>
    <Drawer
      title="添加联系记录"
      v-model="formMode2New.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formMode2New.fields"
        ref="formdispatLogNew"
        :rules="formMode2New.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="客户" prop="clientName">
            <Input v-model="formMode2New.fields.clientName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系人" prop="contactName">
            <Input v-model="formMode2New.fields.contactName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商机" prop="businessName">
            <!-- <Input v-model="formMode2New.fields.businessName" :readonly="true" /> -->
            <Select v-model="formMode2New.fields.businessUuid" filterable>
              <Option
                v-for="item in this.formMode2New.business"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="联系内容" prop="callContent">
            <Input
              type="textarea"
              v-model="formMode2New.fields.callContent"
              placeholder="联系内容"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系时间" prop="callTime">
            <Date-picker
              type="datetime"
              @on-change="formMode2New.fields.callTime = $event"
              :value="datetimeNow"
              format="yyyy-MM-dd HH:mm"
              placeholder="请选择日期和时间"
              style="width: 200px"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem
            label="联系方式"
            prop="contactDetailsUuid"
            style="width: 150px"
          >
            <Select v-model="formMode2New.fields.contactDetailsUuid">
              <Option
                v-for="item in stores.contactPerson.cityListLog"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="lianxirenLogCreate"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formMode2New.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>

<script>
// import JSZip from "jszip";
import FileSaver from "file-saver";
import DzTable from "_c/tables/dz-table.vue";
import {
  loadEditDataXQ,
  getkehu,
  shopInfoImport,
  getAddressInfo,
  deleteCarDriver,
  create2,
  deletelianxijilulog,
  create,
  jiekoushangji,
  editSave, //编辑保存
  loadEditData, //加载编辑
} from "@/api/contact/contactPeople";
import { BusViewlxrLog } from "@/api/customer/myClients";
import config from "@/config";
export default {
  name: "homeaddress",
  components: {
    DzTable,
  },
  data() {
    let globalvalidatePhone = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        let reg = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
        if (!reg.test(value)) {
          callback(new Error("请输入有效的电话号码"));
        }
        callback();
      } else {
        callback(new Error("电话号码不能为空"));
      }
      callback();
    };
    let validateName = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        let reg = /^[^\s]+$/;
        if (!reg.test(value)) {
          callback(new Error("请输入有效的名称"));
        }
        callback();
      } else {
        callback(new Error("名称不能为空"));
      }
      callback();
    };
    return {
      options3: {
        disabledDate(date) {
          return date && date.valueOf() > Date.now();
        },
      },
      usName: "",
      usGuid: "",
      shangjiUuid: "",
      contactName: "",
      clientName: "",
      callLogUuid: "",
      clientUuid: "",
      businessUuid: "",
      businessName: "",
      datetimeNow: "",
      contactPersonUuid: "",
      contactName: "",

      commands: {
        export: { name: "export", title: "导出" },
      },

      //list33: [],

      //导入
      url: config.baseUrl.dev,
      importdisable: false,

      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false,
      },

      // list:[{fileUrl:"",renameFileName:""}],
      // filename:'',

      //form保存参数

      formModeCP: {
        opened: false,
        fields: {
          contactPersonUuid: "",
          title: "",
          call: "",
          phone: "",
          cellphone: "",
          email: "",
          sex: "",
          dateBirth:"",
          addTime: "",
          contactDetailsUUID: "",
          contactAddressUUID: "",
          callLogUUID: "",
          contactName: "",
          remarks: "",
          clientName: "",
          clientUuid: "",
          clientPhone: "",
          familyAddress: "",
          officeAddress: "",
          tencentQq: "",
          zjPhone: "",
          bgPhone: "",
          weChat: "",
          phoneType: "",
          addressType: "",
          phoneText: "",
          addressText: "",
          userGuid: "",
          userName: "",
        },
      },
      formModel: {
        selection: [],
        opened: false,
        mode: "edit",
        fields: {
          contactPersonUuid: "",
          title: "",
          call: "",
          phone: "",
          cellphone: "",
          email: "",
          sex: "",
          dateBirth:"",
          addTime: "",
          contactDetailsUUID: "",
          contactAddressUUID: "",
          callLogUUID: "",
          contactName: "",
          remarks: "",
          clientName: "",
          clientUuid: "",
          clientPhone: "",
          familyAddress: "",
          officeAddress: "",
          tencentQq: "",
          zjPhone: "",
          bgPhone: "",
          weChat: "",
          phoneType: "",
          addressType: "",
          phoneText: "",
          addressText: "",
          userGuid: "",
          userName: "",
        },
        rules: {
          contactName: [
            { type: "string", required: true, validator: validateName },
          ],
          // phone: [
          //   { type: "string", required: true, validator: globalvalidatePhone }
          // ],
          clientUuid: [
            { type: "string", required: true, message: "请选择客户" },
          ],
        },
      },
      formMode2: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          contactPersonUuid: "",
          title: "",
          call: "",
          phone: "",
          cellphone: "",
          email: "",
          sex: "",
          addTime: "",
          contactDetailsUUID: "",
          contactAddressUUID: "",
          callLogUUID: "",
          contactName: "",
          remarks: "",
          clientName: "",
          clientUuid: "",
          clientPhone: "",
          dateBirth:"",
          familyAddress: "",
          officeAddress: "",
          tencentQq: "",
          zjPhone: "",
          bgPhone: "",
          weChat: "",
          phoneType: "",
          addressType: "",
          phoneText: "",
          addressText: "",
          userGuid: "",
          userName: "",
        },
        rules: {
          contactName: [
            { type: "string", required: true, validator: validateName },
          ],
          // phone: [
          //   { type: "string", required: true, validator: globalvalidatePhone }
          // ],
          clientUuid: [
            { type: "string", required: true, message: "请选择客户" },
          ],
        },
      },
      formModel3: {
        opened: false,
        title: "创建申请",
        mode: "",
        selection: [],
        fields: {
          towns: "",
          vname: "",
          villageId: "",
        },
      },

      formMode2New: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          callLogUuid: "",
          clientUuid: "",
          contactPersonUuid: "",
          contactName: "",
          callContent: "",
          callTime: "",
          callResults: "",
          remarks: "",
          clientName: "",
          businessUuid: "",
          businessUuidName: "",
          businessName: "",
          contactDetailsUuid: "",
          contactDetailsText: "",
          userName: "",
          userGuid: "",
        },
        rules: {
          callContent: [
            { type: "string", required: true, message: "请填写联系内容" },
          ],
          callTime: [
            { type: "string", required: true, message: "请选择联系时间" },
          ],
        },
        contact: [],
        client: [],
        business: [],
      },

      //form保存参数
      formModelLog: {
        selection: [],
        opened: false,
        title: "联系人联系记录列表",
        buslist: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDelete: 0,
            status: -1,
            kw: "",
            kw1: "",
            guid: "",
            ClientUUID: "",
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {},
          columns: [
            { type: "selection", width: 50, key: "businessUuid" },

            {
              title: "联系人",
              key: "contactName",
              minWidth: 80,
              sortable: true,
            },
            {
              title: "所属客户",
              key: "clientName",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "商机名称",
              key: "businessName",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "联系内容",
              key: "callContent",
              minWidth: 200,
              sortable: true,
            },
            {
              title: "联系时间",
              key: "callTime",
              width: 120,
              sortable: true,
            },
            {
              title: "联系方式",
              key: "contactDetailsName",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "操作",
              align: "center",
              fixed: "right",
              minWidth: 50,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
        //自定义样式
        styles: {},
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
                label: "",
              },
            ],
            clientNameNew: [
              {
                value: "",
                label: "",
              },
            ],
          },

          cityList: [
            {
              value: "1",
              label: "男",
            },
            {
              value: "2",
              label: "女",
            },
            {
              value: "3",
              label: "未知",
            },
          ],
          // model1: "",

          cityList1: [
            {
              value: "weChat",
              label: "微信",
            },
            {
              value: "tencentQQ",
              label: "QQ",
            },
            {
              value: "phone",
              label: "手机号",
            },
            {
              value: "cellphone",
              label: "座机号码",
            },
          ],
          // model1: "",

          cityList2: [
            {
              value: "officeAddress",
              label: "办公地址",
            },
            {
              value: "familyAddress",
              label: "家庭地址",
            },
          ],
          cityListLog: [
            {
              value: "微信",
              label: "微信",
            },
            {
              value: "QQ",
              label: "QQ",
            },
            {
              value: "电话",
              label: "电话",
            },
            {
              value: "面谈",
              label: "面谈",
            },
            {
              value: "邮件",
              label: "邮件",
            },
          ],
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
                field: "ID",
              },
            ],
          },
          //请求获得的数据集合,用于填入表格
          data: [],
          //一些自定义附件处理数据,用于下拉列表之类

          // url:'',

          //table的列项名称
          columns: [
            //选择框
            { type: "selection", width: 35, key: "handle" },
            {
              title: "联系人名称",
              key: "contactName",
              minWidth: 95,
              maxWidth:100,
              sortable: true,
            },
            {
              title: "所属客户",
              key: "clientName",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "客户经理",
              key: "clientManager",
              minWidth: 85,
              maxWidth:90,

              sortable: true,
            },
            {
              title: "职务",
              key: "call",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "生日",
              key: "dateBirth",
              minWidth: 80,
              maxWidth:90,
              sortable: true,
            },
            { title: "电话", key: "phone", minWidth: 100, sortable: true },
            {
              title: "操作",
              align: "center",
              fixed: "right",
              width: 120,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
        xxx2: {},
        //...
      },
      //自定义样式
      styles: {},
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
      return this.formModel.selection.map((x) => x.homeAddressUuid);
    },
    //...
  },
  //方法集合
  methods: {
    showInfo_xxx(row) {
      this.formModeCP.opened = true;
      loadEditDataXQ({ guid: row.contactPersonUuid }).then((res) => {
        console.log(res);
        this.formModeCP.fields = res.data.data;
      });
    },

    //添加（保存）
    docreateLog() {
      //console.log(this.formModel.fields);
      create2(this.formMode2New.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleResetFormlxrLog();
          this.handleCloselxrLog(); //关闭表单
          this.formMode2New.opened = false;
          this.formModelLog.opened = false;
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    lianxirenLogCreate() {
      let valid = this.validateDispatchLog();
      if (valid) {
        if (this.formMode2New.mode === "create") {
          this.docreateLog();
        }
      }
    },
    //非空验证提示
    validateDispatchLog() {
      let _valid = false;
      this.$refs["formdispatLogNew"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    //联系记录删除
    lianxijiluDelete(row) {
      this.DeleLog(row.callLogUuid);
    },
    DeleLog(ids) {
      console.log(ids);
      deletelianxijilulog({
        ids: ids,
        usName: this.$store.state.user.userName,
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formMode2New.opened = false;
          this.formModelLog.opened = false;
          this.loadListAddress();
          //this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    //添加（联系记录）
    handleCreatelianxirenLog() {
      this.handleSwitchLog();
      this.handleOpenlianxirenLog(); //打开窗口
      this.handleResetFormlxrLog(); //清空表单
      this.formMode2New.fields.userName = this.usName;
      this.formMode2New.fields.userGuid = this.usGuid;
      this.formMode2New.fields.contactName = this.contactName;
      this.formMode2New.fields.clientName = this.clientName;
      this.formMode2New.fields.callLogUuid = this.callLogUuid;
      this.formMode2New.fields.clientUuid = this.clientUuid;
      this.formMode2New.fields.contactPersonUuid = this.contactPersonUuid;
      this.formMode2New.fields.contactName = this.contactName;
      this.lianxirenLogshangji(this.formMode2New.fields.clientUuid);
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
      this.formMode2New.fields.callTime = this.datetimeNow;
      //this.shangjiLog();
    },
    //获取当前商机所属客户的全部联系人
    lianxirenLogshangji(guid) {
      jiekoushangji({ guid: guid }).then((res) => {
        console.log(res);
        this.formMode2New.business = res.data.data;
      });
    },

    //当前商机的所有联系记录
    bushandleLog(row) {
      this.formModelLog.opened = true;
      this.$refs["fromcreat12"].resetFields();
      this.contactName = row.contactName;
      this.clientName = row.clientName;
      this.callLogUuid = row.callLogUuid;
      this.clientUuid = row.clientUuid;
      this.businessUuid = row.businessUuid;
      (this.contactPersonUuid = row.contactPersonUuid),
        (this.contactName = row.contactName),
        (this.formModelLog.buslist.query.guid = row.contactPersonUuid);
      BusViewlxrLog(this.formModelLog.buslist.query).then((res) => {
        console.log(res);
        this.formModelLog.buslist.data = res.data.data;
        this.formModelLog.buslist.query.totalCount = res.data.totalCount;
      });
    },

    handleToken() {
      btnToken().then((res) => {
        console.log(res.data);
      });
    },

    //加载表格数据
    loadListAddress() {
      // console.log(111);

      this.usName = this.$store.state.user.userName;
      this.usGuid = this.$store.state.user.userGuid;
      this.stores.contactPerson.query.kw2 = this.$store.state.user.userGuid;
      //console.log(this.usGuid);
      getAddressInfo(this.stores.contactPerson.query).then((res) => {
        console.log(res.data);
        this.stores.contactPerson.data = res.data.data;
        this.stores.contactPerson.query.totalCount = res.data.totalCount;
      });
    },

    //导出相关操作
    handleExportInfo(command) {
      console.log(this.selectRowsId);

      if (!this.selectRowsId || this.selectRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要生成并 [" +
          this.commands[command].title +
          "] 当前已选地址的家庭码?</p>",
        loading: true,
        onOk: () => {
          this.doExportInfoCommand(command);
        },
      });
    },
    doExportInfoCommand(command) {
      exportInfoCommand({
        command: command,
        ids: this.selectRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          // console.log(res.data);
          var DZurl = res.data.data;
          window.location = this.url + DZurl;
          this.$Message.success(res.data.message);
          this.loadListAddress();
          // this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },

    //一键导出
    selectYjexportInfo() {
      yjexportInfo(this.stores.contactPerson.query).then((res) => {
        //this.stores.contactPerson.data = res.data.data;
        var DZurl = res.data.data;
        window.location = this.url + DZurl;
        this.loadListAddress();
        this.$Message.success("一键导出成功");
      });
    },

    //导入相关操作
    handleImportUserInfo() {
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      this.$refs.inputer.value = "";
      this.isexitexcel = false;
      this.formimport.opened = true;
    },
    handleimportmodel() {
      //console.log(this.url);
      window.location.href =
        this.url +
        "UploadFiles/Templates/联系人基础信息导入模板.xlsx";
    },
    //导入
    importfxx(e) {
      let inputDOM = this.$refs.inputer;
      //console.log(inputDOM);
      //声明一个FormDate对象
      let formData = new FormData();
      let file = inputDOM.files[0];
      let AllUpExt = ".xls|.xlsx|";
      let extName = file.name
        .substring(file.name.lastIndexOf("."))
        .toLowerCase();
      if (AllUpExt.indexOf(extName + "|") == "-1") {
        this.$refs.inputer.value = "";
        this.$Message.warning("文件格式不正确!");
      } else {
        if (file != null && file != undefined) {
          this.isexitexcel = true;
          formData.append("excelFile", file);
          // formData.append("fuzeren", this.$store.state.user.userGuid);
          // console.log(file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
      // console.log(this.exceldata);
    },
    async handleimport() {
      this.importdisable = true;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      console.log(111);
      if (this.isexitexcel) {
        // let fuzeren = this.$store.state.user.userGuid;
        // let data={
        //   fuzeren:this.$store.state.user.userGuid,
        //   exceldata:this.exceldata
        // }

        await shopInfoImport(this.exceldata).then((res) => {
          //console.log(res.data);
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadListAddress();
          } else {
            this.$Message.warning(res.data.message);
            //clearInterval(this.intervalId);
            //this.showprogress = false;
          }
          this.$refs.inputer.value = "";
          this.exceldata = new FormData();
          this.isexitexcel = false;
        });
        //clearInterval(this.intervalId);
        //this.showprogress = false;
      } else {
        this.$Message.warning("请选择文件");
      }
      this.importdisable = false;
    },

    //添加按钮（志愿者申请）
    handleShowCreateWindow() {
      console.log(this.usName);
      this.formMode2.fields.userName = this.usName;
      this.formMode2.fields.userGuid = this.usGuid;
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      //this.formMode2.fields.realName = this.$store.state.user.userName;
    },
    handleSwitchFormModeToCreate() {
      this.formMode2.mode = "create";
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatccc2"].resetFields();
    },
    //添加志愿者（保存）
    docreateCarDispatch() {
      //console.log(this.formModel.fields);
      this.formMode2.fields.usName = this.$store.state.user.userName;
      this.formMode2.fields.usGuid = this.$store.state.user.userGuid;
      create(this.formMode2.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleResetFormDispatch();
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
      this.$refs["formdispatccc2"].validate((valid) => {
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

    //自动关闭窗口
    handleCloseFormWindow() {
      this.formMode2.opened = false;
    },

    //右边编辑
    handleEdit(row) {
      // console.log(row);
      // this.formModel.fields.userName = this.usName;
      // this.formModel.fields.userGuid = this.usGuid;
      this.handleOpenFormWindow1();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch1();
      this.doLoadEditData(row.contactPersonUuid);
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
    },
    doLoadEditData(contactPersonUuid) {
      loadEditData({ guid: contactPersonUuid }).then((res) => {
        console.log(res.data);
        //this.stores.contactPerson.sources.clientName = res.data.data.clientName;
        this.formModel.fields = res.data.data;
      });
    },

    //编辑（保存）
    doEditDispatch() {
      // var reg = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
      // if (!reg.test(this.formModel.fields.phone)) {
      //   this.$Message.warning("电话不合法");
      //   return;
      // }
      // console.log(this.formModel.fields);
      this.formModel.fields.usName = this.$store.state.user.userName;
      this.formModel.fields.usGuid = this.$store.state.user.userGuid;
      editSave(this.formModel.fields).then((res) => {
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
      //console.log(row.clientUuid);
      this.doDelete(row.contactPersonUuid);
    },
    doDelete(ids) {
      console.log(ids);
      deleteCarDriver({
        ids: ids,
        usName: this.$store.state.user.userName,
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadListAddress();
          //this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    handleSwitchLog() {
      this.formMode2New.mode = "create";
    },
    //打开窗口
    handleOpenlianxirenLog() {
      this.formMode2New.opened = true;
    },
    //清空
    handleResetFormlxrLog() {
      this.$refs["formdispatLogNew"].resetFields();
    },
    //自动关闭窗口
    handleCloselxrLog() {
      this.formMode2New.opened = false;
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
    //翻页
    handlePageChanged4(page) {
      this.formModel4.buslist.query.currentPage = page;
      this.loadListAddress();
    },
    //显示条数改变
    handlePageSizeChanged4(pageSize) {
      this.formModel4.buslist.query.pageSize = pageSize;
      this.loadListAddress();
    },
    BushandleSelect1(selection, row) {},
    //多选
    BushandleSelectionChange1(selection) {
      this.formModelLog.selection = selection;
    },
    doCancel_xxx() {
      this.formModelLog.opened = false;
    },
    rowClsRender_xxx(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },
    //申请下拉框
    loadDepartmentList1() {
      //let data = this.formMode2.fields.clientName;
      this.usGuid = this.$store.state.user.userGuid;
      let manGuid = this.$store.state.user.userGuid;
      getkehu().then((res) => {
        console.log(res.data.data);
        this.stores.contactPerson.sources.clientName = res.data.data;
        //console.log(this.stores.contactPerson.sources.clientName);
      });
    },

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
    },
  },

  //页面加载时执行
  mounted() {
    this.loadDepartmentList1();
    this.loadListAddress();
  },

  computed: {
    optionsA() {
      let _this = this;
      let optionsA = [];
      if (this.searchBox === "") {
        optionsA = this.propsSingle.options;
      } else {
        this.propsSingle.options.forEach(function (item) {
          if (item.sparticipantfullname.search(_this.inputStr) != -1) {
            optionsA.push(item);
          }
        });
      }
      return optionsA;
    },
  },
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
