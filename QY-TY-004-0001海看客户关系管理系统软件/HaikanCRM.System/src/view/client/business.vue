<template>
  <div>
    <Card>
      <Form
        :model="formModel4.fields"
        ref="fromcreat1"
        :rules="formModel4.rules"
        label-position="top"
      >
        <dz-table
          class="carlist"
          :totalCount="formModel4.buslist.query.totalCount"
          :pageSize="formModel4.buslist.query.pageSize"
          @on-page-change="handlePageChanged2"
          @on-page-size-change="handlePageSizeChanged2"
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
                        v-model="formModel4.buslist.query.kw"
                        placeholder="输入商机名称"
                        @on-search="handleSearchDispatch()"
                      ></Input>
                      <Input
                        style="width: 150px"
                        type="text"
                        search
                        :clearable="true"
                        v-model="formModel4.buslist.query.kw1"
                        placeholder="输入所属客户名称"
                        @on-search="handleSearchDispatch()"
                      ></Input>
                      <Select
                        filterable
                        :clearable="true"
                        v-model="formModel4.buslist.query.stasources"
                        @on-change="handleSearchDispatch"
                        placeholder="阶段"
                        style="width: 150px"
                      >
                        <Option
                          v-for="item in formModel4.buslist.sources
                            .stateSources"
                          :value="item.value"
                          :key="item.value"
                          >{{ item.text }}</Option
                        >
                      </Select>
                    </FormItem>
                  </Form>
                </Col>
                <Col span="8" class="dnc-toolbar-btns">
                  <ButtonGroup class="mr3">
                    <Button
                      v-can="'delete'"
                      class="txt-danger"
                      icon="md-trash"
                      title="删除"
                      @click="handleBatchCommand('delete')"
                    ></Button>
                  </ButtonGroup>
                  <Button
                    v-can="'buscreate'"
                    icon="md-create"
                    type="primary"
                    @click="handleBusCreateWindow"
                    title="添加商机信息"
                    >添加商机信息</Button
                  >
                  <ButtonGroup class="mr3">
                    <Button
                      icon="md-refresh"
                      title="刷新"
                      @click="loadBusList"
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
            @on-select="BushandleSelect"
            @on-selection-change="BushandleSelectionChange"
            :data="formModel4.buslist.data"
            :columns="formModel4.buslist.columns"
            :row-class-name="rowClsRender_xxx"
          >
            <template slot-scope="{ row, index }" slot="state">
              <span>{{ CheckState(row.state) }}</span>
            </template>
            <template slot-scope="{ row, index }" slot="action">
              <Tooltip
                placement="top"
                content="编辑"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  v-can="'busedit'"
                  type="primary"
                  size="small"
                  shape="circle"
                  icon="md-create"
                  @click="bushandleEdit(row)"
                ></Button>
              </Tooltip>
              <Tooltip
                placement="top"
                content="详情"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  v-can="'busview'"
                  type="success"
                  size="small"
                  shape="circle"
                  icon="md-search"
                  @click="bushandleView(row)"
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
                content="商机文件"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  v-can="'busFile'"
                  type="primary"
                  size="small"
                  shape="circle"
                  icon="ios-cloud-upload"
                  @click="bushandleFile(row)"
                ></Button>
              </Tooltip>
            </template>
          </Table>
        </dz-table>
      </Form>
    </Card>

    <Drawer
      :title="formTitle1"
      v-model="busEditModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="busEditModel.mobile"
        ref="BusDispatchDetail"
        label-position="left"
        :rules="busEditModel.rules"
      >
        <Row :gutter="16">
          <FormItem label="商机名称:" prop="businessName">
            <Input
              v-model="busEditModel.mobile.businessName"
              placeholder="请输入商机名称"
              style="width: 85%"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="所属客户" prop="clientUuid">
            <Select
              v-model="busEditModel.mobile.clientUuid"
              style="width: 400px"
              filterable
              @on-change="lxr"
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
        <Row :gutter="16">
          <FormItem label="相关联系人" prop="roleUuid" v-if="kaiguan">
            <Select v-model="lxrcunlist" multiple style="width: 400px" @on-change="testlxr">
              <Option
                v-for="item in lxrlist"
                :value="item.contactPersonUuid"
                :key="item.contactPersonUuid"
                >{{ item.contactName }}</Option
              >
            </Select>
            <!-- <Checkbox-group v-model="lxrcunlist" @on-change="testlxr">
              <Checkbox
                class="cbox"
                v-for="it in lxrlist"
                :label="it.contactPersonUuid"
              >
                <span>{{ it.contactName }}</span>
              </Checkbox>
            </Checkbox-group> -->
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12" style="margin-left: -8px">
            <FormItem label="商机来源:" prop="businessSource">
              <Select
                style="width: 150px"
                v-model="busEditModel.mobile.businessSource"
              >
                <Option
                  v-for="item in busEditModel.pickerSource"
                  :value="item"
                  :key="item"
                  >{{ item }}</Option
                >
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="商机类型:" prop="businessType">
              <Select
                style="width: 150px"
                v-model="busEditModel.mobile.businessType"
              >
                <Option
                  v-for="item in busEditModel.pickerType"
                  :value="item"
                  :key="item"
                  >{{ item }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="项目预算:" prop="salesAmount">
            <Input
              v-model="busEditModel.mobile.salesAmount"
              placeholder="请输入项目预算"
              style="width: 85%"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12" style="margin-left: -8px">
            <FormItem label="商机赢率:" prop="winrate">
              <Select
                style="width: 150px"
                v-model="busEditModel.mobile.winrate"
              >
                <Option
                  v-for="item in busEditModel.pickerWinrate"
                  :value="item"
                  :key="item"
                  >{{ item }}</Option
                >
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="商机阶段:" prop="businessStage">
              <Select
                style="width: 150px"
                v-model="busEditModel.mobile.businessStage"
              >
                <Option
                  v-for="item in busEditModel.pickerStage"
                  :value="item"
                  :key="item"
                  >{{ item }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="预计成交时间" prop="checkTime">
            <DatePicker
              v-model="busEditModel.mobile.checkTime"
              @on-change="busEditModel.mobile.checkTime = $event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="预计成交时间"
              style="width: 150px"
            ></DatePicker>
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="参与人" prop="systemUserName">
            <Input
              v-model="busEditModel.mobile.systemUserName"
              @on-change="selectcanyu"
              style="width: 150px;"
              placeholder="请选择参与人"
              :readonly="true"
            />
            <i-button type="info" @click="selectcanyu(rowid)">选择参与人</i-button>
          </FormItem>
        </Row>-->
        <Row :gutter="16">
          <FormItem label="备注:">
            <Input
              type="textarea"
              v-model="busEditModel.mobile.remarks"
              placeholder="备注信息"
              style="width: 85%; margin-left: 4%"
            />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="busClick"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="busEditModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>

    <Drawer
      title="查看商机信息"
      v-model="busViewModel.opened"
      width="600"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="busViewModel.query"
        ref="formCarDispatchDetail"
        label-position="left"
      >
        <Row :gutter="16">
          <FormItem label="商机名称">
            <Input v-model="busViewModel.query.businessName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="所属客户">
            <Input v-model="busViewModel.query.clientName" :readonly="true" />
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="客户经理">
            <Input v-model="busViewModel.query.realName" :readonly="true" />
          </FormItem>
        </Row>-->
        <!-- <Row :gutter="16">
          <FormItem label="参与人">
            <Input v-model="busViewModel.query.systemUserName" :readonly="true" />
          </FormItem>
        </Row>-->

        <Row :gutter="16">
          <FormItem label="商机来源">
            <Input
              v-model="busViewModel.query.businessSource"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商机类型">
            <Input v-model="busViewModel.query.businessType" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="项目预算">
            <Input v-model="busViewModel.query.salesAmount" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商机赢率">
            <Input v-model="busViewModel.query.winrate" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商机阶段">
            <Input
              v-model="busViewModel.query.businessStage"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="预计成交时间">
            <Input v-model="busViewModel.query.checkTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注">
            <Input v-model="busViewModel.query.remarks" :readonly="true" />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="busViewModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
    <!-- <Drawer
      title="选择参与人"
      v-model="formModelcanyu.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModelcanyu.fields" ref="formselect" label-position="top">
        <FormItem label="参与人" prop="participant">
          <Transfer
            :data="formModelcanyu.fields.participantleft"
            :target-keys="formModelcanyu.fields.participantright"
            :list-style="{width: '240px',height: '400px'}"
            :titles="['所有人员','选择的人员']"
            filterable
            @on-change="handleSchoolChange"
          ></Transfer>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="formModelcanyu.opened = false">确 定</Button>
      </div>
    </Drawer>-->
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
                    v-can="'delete'"
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
        ref="formdispatccc2"
        :rules="formMode2New.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="客户" prop="clientName">
            <Input v-model="formMode2New.fields.clientName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商机" prop="businessName">
            <Input
              v-model="formMode2New.fields.businessName"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系人" prop="contactPersonUuid">
            <Select v-model="formMode2New.fields.contactPersonUuid" filterable>
              <Option
                v-for="item in this.formMode2New.contact"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
            <!-- <Input v-model="formMode2New.fields.contactName" :readonly="true" /> -->
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
            <!-- <Input type="textarea" v-model="formMode2New.fields.callTime" placeholder="联系内容" /> -->
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
                v-for="item in stores.contactPerson.sources.cityList1"
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

    <Drawer
      title="商机文件目录"
      v-model="busModelFile.opened"
      width="900"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="busModelFile.fields"
        ref="filefrombus"
        :rules="busModelFile.rules"
        label-position="top"
      ></Form>
      <Card>
        <dz-table
          :totalCount="stores.file.query.totalCount"
          :pageSize="stores.file.query.pageSize"
          @on-page-change="handleFilePageChanged"
          @on-page-size-change="handleFilePageSizeChanged"
        >
          <div slot="searcher">
            <section class="dnc-toolbar-wrap">
              <Row :gutter="16">
                <Col span="16">
                  <Form inline @submit.native.prevent>
                    <FormItem>
                      <Input
                        type="text"
                        search
                        :clearable="true"
                        v-model="stores.file.query.kw"
                        placeholder="输入关键字搜索..."
                        @on-search="handleSearchFile()"
                      >
                        <!-- <Select
                          slot="prepend"
                          v-model="stores.file.query.isDeleted"
                          @on-change="handleSearchFile"
                          placeholder="删除状态"
                          style="width:60px;"
                        >
                          <Option
                            v-for="item in stores.file.sources.isDeletedSources"
                            :value="item.value"
                            :key="item.value"
                          >{{item.text}}</Option>
                        </Select>-->
                      </Input>
                    </FormItem>
                  </Form>
                </Col>
                <Col span="8" class="dnc-toolbar-btns">
                  <ButtonGroup class="mr3">
                    <Button
                      v-can="'delete'"
                      class="txt-danger"
                      icon="md-trash"
                      title="删除"
                      @click="handleFileBatchCommand('delete')"
                    ></Button>
                    <!-- <Button
                      v-can="'huifu'"
                      class="txt-success"
                      icon="md-redo"
                      title="恢复"
                      @click="handleBatchCommand('recover')"
                    ></Button>-->

                    <Button
                      icon="md-refresh"
                      title="刷新"
                      @click="handleFileRefresh"
                    ></Button>
                  </ButtonGroup>
                  <Button
                    v-can="'create'"
                    icon="md-create"
                    type="primary"
                    @click="handleFileShowCreateWindow"
                    title="新增文件"
                    >新增文件</Button
                  >
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
            :data="stores.file.data"
            :columns="stores.file.columns"
            @on-select="handleFileSelect"
            @on-selection-change="handleFileSelectionChange"
            @on-refresh="handleFileRefresh"
            :row-class-name="rowClsRenderFile"
            @on-page-change="handleFilePageChanged"
            @on-page-size-change="handleFilePageSizeChanged"
            @on-sort-change="handleFileSortChange"
          >
            <template slot-scope="{ row, index }" slot="fileAction">
              <Poptip
                confirm
                :transfer="true"
                title="确定要删除吗?"
                @on-ok="handleFileDelete(row)"
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
                    v-can="'delete1'"
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
                  type="primary"
                  size="small"
                  shape="circle"
                  icon="md-create"
                  @click="handleFileEdit(row)"
                ></Button>
              </Tooltip>
              <Button
                type="primary"
                size="small"
                shape="circle"
                icon="md-arrow-down"
                @click="handleFileDown(row)"
              ></Button>
            </template>
          </Table>
        </dz-table>
      </Card>
    </Drawer>
    <Drawer
      :title="formFileTitle"
      v-model="busModelFile.formopened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="busModelFile.fields"
        ref="formFile"
        :rules="busModelFile.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="文件名称" prop="fileName">
            <Input
              v-model="busModelFile.fields.fileName"
              :readonly="busModelFile.mode == 'show'"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="文件简介" prop="fileInfo">
            <Input
              :autosize="true"
              v-model="busModelFile.fields.fileInfo"
              :readonly="busModelFile.mode == 'show'"
              type="textarea"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Upload
            ref="upload"
            :action="actionurl"
            :headers="postheaders"
            :show-upload-list="true"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            style="float: left"
            :disabled="updisabled"
            :on-remove="deleteFile"
          >
            <Button
              type="primary"
              icon="ios-cloud-upload-outline"
              :loading="loadingStatus"
              >上传文件</Button
            >
          </Upload>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitFile"
          >保 存</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="busModelFile.formopened = false"
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
  getkehu,
  jiekoulxr,
  shopInfoImport,
  getAddressInfo,
  deleteCarDriver,
  deletelianxijilulog,
  loadSimpleList,
  create,
  create2,
  editSave, //编辑保存
  loadEditData, //加载编辑
} from "@/api/contact/contactPeople";
import {
  loadEditData1, //加载商机
  editSaveBusi, //保存商机
  EditContactPerson,
  binddataok,
  binddataok1,
  getcanyuren,
  BusView,
  BusView2,
  BusView3,
  BusEdit,
  gealluser,
  BusAdd,
  Batch,
  loadEditData1sahngji,
} from "@/api/customer/myClients";
import {
  getFileList,
  createFile,
  loadFile,
  editFile,
  deleteFile,
  batchCommandFile,
} from "@/api/business/bDocuments";
import config from "@/config";
import { getToken } from "@/libs/util";
import { globalvalidateIsNotEmpty } from "@/global/validate";
export default {
  name: "homeaddress",
  components: {
    DzTable,
  },
  data() {
    return {
      datetimeNow: "",
      userGuid: "",
      userName: "",
      panduanrowid: 0,
      rowid: "", //选择行的id
      usName: "",
      usGuid: "",
      shangjiUuid: "",
      contactName: "",
      clientName: "",
      callLogUuid: "",
      clientUuid: "",
      businessUuid: "",
      businessName: "",
      lxrlist: [],
      lxrcunlist: [],
      lclist: [],
      connamelist: [],
      kaiguan: true,

      commands: {
        delete: { name: "delete", title: "删除" },
      },

      // list:[{fileUrl:"",renameFileName:""}],
      // filename:'',

      //form保存参数
      formModelcanyu: {
        opened: false,
        title: "选择人员",
        mode: "select",
        fields: {
          participantleft: [],
          participantright: [],
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
          callContent: "",
          callTime: "",
          callResults: "",
          remarks: "",
          contactName: "",
          clientName: "",
          businessUuid: "",
          businessUuidName: "",
          businessName: "",
          contactDetailsUuid: "",
          contactDetailsText: "",
          userName: "",
          userGuid: "",
          contactBusName: "",
        },
        rules: {
          callContent: [
            { type: "string", required: true, message: "请填写联系内容" },
          ],
          callTime: [
            { type: "string", required: true, message: "请选择联系时间" },
          ],
          contactPersonUuid: [
            { type: "string", required: true, message: "请选择联系人" },
          ],
        },
        contact: [],
        client: [],
        business: [],
      },
      //form保存参数
      formModel4: {
        selection: [],
        opened: false,
        title: "商机列表",
        buslist: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDelete: 0,
            status: -1,
            stasources: "",
            kw: "",
            kw1: "",
            sysUuid: "",
            ClientUUID: "",
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            stateSources: [
              { value: "沟通调研", text: "沟通调研" },
              { value: "方案策划", text: "方案策划" },
              { value: "招投标", text: "招投标" },
              { value: "合同签订", text: "合同签订" },
              { value: "已关闭", text: "已关闭" },
            ],
          },
          columns: [
            { type: "selection", width: 35, key: "businessUuid" },
            {
              title: "商机名称",
              key: "businessName",
              minWidth: 100,
              
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
              minWidth: 90,
              maxWidth:100,
              sortable: true,
            },
            {
              title: "相关联系人",
              key: "systemUserName",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "项目预算",
              key: "salesAmount",
              minWidth: 95,
              maxWidth:100,
              sortable: true,
            },

            {
              title: "商机阶段",
              key: "businessStage",
              minWidth: 80,
              maxWidth:110,
              sortable: true,
            },
            {
              title: "预计成交时间",
              key: "checkTime",
              minWidth: 100,
              maxWidth:150,

              sortable: true,
            },
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
        //自定义样式
        styles: {},
      },

      //form保存参数
      formModelLog: {
        selection: [],
        opened: false,
        title: "商机联系记录列表",
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
              title: "商机名称",
              key: "businessName",
              minWidth: 100,
              sortable: true,
            },
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
              title: "联系内容",
              key: "callContent",
              minWidth: 200,
              sortable: true,
            },
            {
              title: "联系时间",
              key: "callTime",
              minWidth: 140,
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
      busEditModel: {
        opened: false,
        title: "修改商机",
        mode: "edit",
        Prioritylist: {
          picker: [
            {
              clientUuid: "",
              clientName: "",
            },
          ],
          index2: -1,
        },
        pickerSource: ["独立开发", "客户介绍", "其它广告", "来电咨询"],
        pickerType: ["普通机会", "重要机会", "特殊机会"],
        pickerStage: ["沟通调研", "方案策划", "招投标", "合同签订", "已关闭"],
        pickerWinrate: [],
        mobile: {
          clientUuid: "",
          businessStage: "",
          salesAmount: "",
          winrate: "",
          businessSource: "",
          businessType: "",
          checkTime: "",
          remarks: "",
          businessName: "",
          businessUuid: "",
          systemUserName: "",
          systemUserUuid: "",
          userName: "",
          userGuid: "",
          contactBusName: "",
        },
        rules: {
          businessName: [
            { type: "string", required: true, message: "请输入名称" },
          ],
          clientUuid: [
            { type: "string", required: true, message: "请选择客户" },
          ],
        },
      },
      busViewModel: {
        selection: [],
        title: "查看商机",
        opened: false,

        mode: "",
        query: {
          businessName: "",
          businessSource: "",
          businessType: "",
          salesAmount: "",
          winrate: "",
          businessStage: "",
          checkTime: "",
          remarks: "",
          clientName: "",
          realName: "",
          systemUserName: "",
        },
      },
      //-----------文件目录-----------
      url: "",
      actionurl: "",
      postheaders: "",
      updisabled: false,
      loadingStatus: false,
      busModelFile: {
        dFileName: "",
        selection: [],
        title: "商机文件目录",
        opened: false,
        formopened: false,
        mode: "",
        fields: {
          busDocumentsUuid: "",
          fileName: "",
          fileInfo: "",
          fileUrl: "",
          businessUuid: "",
          clientUuid: "",
          contactPersonUuid: "",
          addTime: "",
          isDelete: "",
        },
        rules: {
          fileName: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入文件名",
            }, // min:3
          ],
          // systemRoleUuid: [
          //   { type: "string", required: true, message: "请选择角色" }
          // ],
          // passWord: [{ type: "string", required: true, message: "请输入密码" }],
          // userIdCard: [
          //   { type: "string", required: true, message: "请输入身份证号码" }
          // ]
        },
      },
      //用于提交及一些保持性数据
      stores: {
        //selection: [],
        //该实例对象相关数据
        contactPerson: {
          sources: {
            clientName: [
              {
                value: "",
                label: "",
              },
            ],
            cityList1: [
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
          },

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
            { type: "selection", width: 50, key: "businessUuid" },
            {
              title: "商机名称",
              key: "businessName",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "商机赢率",
              key: "winrate",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "商机阶段",
              key: "businessStage",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "预计成交时间",
              key: "checkTime",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "操作",
              align: "center",
              fixed: "right",
              width: 80,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
        file: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            businessUuid: "",
            kw: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            // userTypes: [
            //   { value: 0, text: "超级管理员" },
            //   { value: 1, text: "管理员" },
            //   { value: 2, text: "普通用户" }
            // ],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            {
              title: "文件名称",
              key: "fileName",
              minWidth: 100,
              sortable: true,
            },
            {
              title: "文件简介",
              key: "fileInfo",
              minWidth: 100,
              ellipsis: true,
            },
            {
              title: "上传时间",
              key: "addTime",
              minWidth: 80,
              maxWidth:140,
            },
            {
              title: "操作",
              align: "center",
              fixed: "right",
              width: 120,
              className: "table-command-column",
              slot: "fileAction",
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
    formTitle1() {
      if (this.busEditModel.mode === "create") {
        return "添加商机信息";
      }
      if (this.busEditModel.mode === "edit") {
        return "编辑商机信息";
      }
      return "";
    },
    formFileTitle() {
      if (this.busModelFile.mode === "create") {
        return "添加商机文件信息";
      }
      if (this.busModelFile.mode === "edit") {
        return "编辑商机文件信息";
      }
      if (this.busModelFile.mode === "show") {
        return "商机文件信息详情";
      }
      return "";
    },
    //删除
    selectRusRowsId() {
      return this.formModel4.selection.map((x) => x.businessUuid);
    },
    selectFileRowsId() {
      return this.busModelFile.selection.map((x) => x.busDocumentsUuid);
    },
  },
  //方法集合
  methods: {
    testlxr(data) {
      this.formMode2New.fields.contactBusName = data.join(",");
      this.busEditModel.mobile.contactBusName = data.join(",");
      console.log(data);
    },
    lxr(e) {
      // console.log(e);
      if (e != null) {
        this.doloadlxrList(e);
      }
    },
    //加载表格数据
    loadListAddress() {
      console.log(12356);
      this.usName = this.$store.state.user.userName;
      this.usGuid = this.$store.state.user.userGuid;
      this.formModel4.buslist.query.sysUuid = this.$store.state.user.userGuid;
      loadEditData1sahngji(this.formModel4.buslist.query).then((res) => {
        console.log(res);
        this.formModel4.buslist.data = res.data.data;
        this.formModel4.buslist.query.totalCount = res.data.totalCount;
      });
    },
    //穿梭框
    loaselect() {
      getcanyuren().then((res) => {
        this.formModelcanyu.fields.participantleft = res.data.data;
        //this.formModelcanyu2.fields.participantleft = res.data.data;
      });
    },
    //打开选择参与人窗口
    selectcanyu(rowid) {
      console.log(rowid);
      this.formModelcanyu.opened = true;
      //修改
      if (rowid != "") {
        binddataok1({ id: rowid }).then((res) => {
          console.log(369);
          if (this.panduanrowid != rowid) {
            //判断是否是本行的数据
            //不是本行数据
            this.formModelcanyu.fields.participantright = []; //清除上次选择的人员
          }
          //如果没有选择人员则显示数据库中的内容（表示选择了新行）
          if (this.formModelcanyu.fields.participantright.length == 0) {
            if (res.data.data.length > 0) {
              for (let i = 0; i < res.data.data.length; i++) {
                //显示数据库保存的人员
                this.formModelcanyu.fields.participantright = this.formModelcanyu.fields.participantright.concat(
                  res.data.data[i].key
                );
              }
            }
          }
          this.panduanrowid = rowid; //将本行的id保存
        });
      }
    },
    //选择参与人
    handleSchoolChange(newTargetKeys, direction, moveKeys) {
      this.formModelcanyu.fields.participantright = newTargetKeys;
      this.busEditModel.mobile.systemUserName = this.busEditModel.mobile.systemUserUuid =
        ""; //每次选择则参与人都重新赋值
      for (
        let i = 0;
        i < this.formModelcanyu.fields.participantright.length;
        i++
      ) {
        for (
          let j = 0;
          j < this.formModelcanyu.fields.participantleft.length;
          j++
        ) {
          if (
            this.formModelcanyu.fields.participantright[i] ===
            this.formModelcanyu.fields.participantleft[j].key
          ) {
            this.busEditModel.mobile.systemUserName +=
              this.formModelcanyu.fields.participantleft[j].label + ",";
            this.busEditModel.mobile.systemUserUuid +=
              this.formModelcanyu.fields.participantleft[j].key + ",";
          }
        }
      }
      console.log(this.formModelcanyu.fields.participantright);
    },
    //商机刷新
    loadBusList() {
      this.loadListAddress();
    },

    //添加（联系记录）
    handleCreatelianxirenLog() {
      this.lxrlist = [];
      this.lxrcunlist = [];
      this.formMode2New.contact = [];
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      this.formMode2New.fields.userName = this.usName;
      this.formMode2New.fields.userGuid = this.usGuid;
      this.formMode2New.fields.contactName = this.contactName;
      this.formMode2New.fields.clientName = this.clientName;
      this.formMode2New.fields.callLogUuid = this.callLogUuid;
      this.formMode2New.fields.clientUuid = this.clientUuid;
      this.formMode2New.fields.businessUuid = this.businessUuid;
      this.formMode2New.fields.businessName = this.businessName;
      this.shangjilianxiren(this.formMode2New.fields.clientUuid);
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
    shangjilianxiren(guid) {
      jiekoulxr({ guid: guid }).then((res) => {
        console.log(res);
        this.formMode2New.contact = res.data.data;
      });
    },

    doloadlxrList(guid) {
      this.lxrcunlist = [];
      loadSimpleList({ guid: guid }).then((res) => {
        console.log(365);
        console.log(res);
        this.lxrlist = res.data.data;
        this.lxrcunlist = this.connamelist;
        // for (let i = 0; i < this.lclist.length; i++) {
        //   let name = this.lxrlist.find(
        //     (x) => x.contactPersonUuid == this.lclist[i]
        //   );
        //   if (typeof name != "undefined") {
        //     this.lxrcunlist.push(name);
        //   }
        // }
        //this.connamelist = this.connamelist;
      });
    },
    shangjiLog() {
      let data = this.shangjiUuid;
      BusView3(data).then((res) => {
        this.formMode2New.fields = res.data.data[0];
        console.log(this.formMode2New.fields);
      });
    },

    handleSwitchFormModeToCreate() {
      this.formMode2New.mode = "create";
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatccc2"].resetFields();
    },
    //添加（保存）
    docreateCarDispatch() {
      //console.log(this.formModel.fields);
      create2(this.formMode2New.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleResetFormDispatch();
          this.handleCloseFormWindow(); //关闭表单
          this.formMode2New.opened = false;
          this.formModelLog.opened = false;
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    lianxirenLogCreate() {
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formMode2New.mode === "create") {
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
      this.formMode2New.opened = true;
    },

    //自动关闭窗口
    handleCloseFormWindow() {
      this.formMode2New.opened = false;
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
        //console.log(res.data);
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
      //console.log(ids);
      deleteCarDriver(ids).then((res) => {
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

    //申请下拉框
    loadDepartmentList1() {
      //let data = this.formMode2New.fields.clientName;
      getkehu().then((res) => {
        //console.log(res);
        //console.log(res.data.data);
        //this.busEditModel.Prioritylist.picker = res.data.data;
        this.stores.contactPerson.sources.clientName = res.data.data;
        //console.log(this.stores.contactPerson.sources.clientName);
      });
    },
    //商机添加
    handleBusCreateWindow() {
      this.lxrlist = [];
      this.lxrcunlist = [];
      this.kaiguan = true;
      this.busEditModel.mobile.userName = this.usName;
      this.busEditModel.mobile.userGuid = this.usGuid;
      this.busEditModel.opened = true;
      this.BusFormModeToCreate();
      this.handleBusDispatchDetail();
      for (let i = 0; i < 101; i++) {
        this.busEditModel.pickerWinrate[i] = i + "%";
      }
      this.busEditModel.mobile.clientUuid = this.formModel4.buslist.query.ClientUUID;
    },

    BusFormModeToCreate() {
      this.busEditModel.mode = "create";
    },
    //清空
    handleBusDispatchDetail() {
      this.$refs["BusDispatchDetail"].resetFields();
    },
    //商机详情
    bushandleView(row) {
      this.busViewModel.opened = true;
      let data = row.businessUuid;
      BusView(data).then((res) => {
        if (res.data.code == 200) {
          this.busViewModel.query = res.data.data[0];
        }
      });
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
          this.loadListAddress();
          this.formMode2New.opened = false;
          this.formModelLog.opened = false;
          //this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    //当前商机的所有联系记录
    bushandleLog(row) {
      this.formModelLog.opened = true;
      this.$refs["fromcreat12"].resetFields();
      this.shangjiUuid = row.businessUuid;
      this.contactName = row.contactName;
      this.clientName = row.clientName;
      this.callLogUuid = row.callLogUuid;
      this.clientUuid = row.clientUuid;
      this.businessUuid = row.businessUuid;
      this.businessName = row.businessName;
      this.formModelLog.buslist.query.guid = row.businessUuid;
      BusView2(this.formModelLog.buslist.query).then((res) => {
        //console.log(res);
        this.formModelLog.buslist.data = res.data.data;
        this.formModelLog.buslist.query.totalCount = res.data.totalCount;
      });
    },
    bushandleFile(row) {
      console.log(11111);
      console.log(row);
      this.stores.file.query.businessUuid = row.businessUuid;
      this.busModelFile.opened = true;
      this.loadFileList();
    },

    BusFormModeToEdit() {
      this.busEditModel.mode = "edit";
    },
    //商机修改
    bushandleEdit(row) {
      this.kaiguan = true;
      this.busEditModel.opened = true;
      this.rowid = row.id; //保存选择行的id
      this.formModelcanyu.fields.participantright = [];
      this.BusFormModeToEdit();
      this.$refs["BusDispatchDetail"].resetFields();
      this.busEditModel.mobile.businessUuid = row.businessUuid;
      gealluser().then((res) => {
        this.busEditModel.Prioritylist.picker = res.data.data;
      });
      for (let i = 0; i < 101; i++) {
        this.busEditModel.pickerWinrate[i] = i + "%";
      }
      let data = row.businessUuid;
      BusView(data).then((res) => {
        if (res.data.code == 200) {
          console.log(221);
          console.log(res);
          if (res.data.data[0].contactBusName != null) {
            this.connamelist = res.data.data[0].contactBusName.split(",");
          } else {
            this.connamelist = [];
          }
          console.log(this.connamelist);
          this.busEditModel.mobile = res.data.data[0];
          console.log(989);
          console.log(res.data.data);
          if (
            res.data.data[0].clientUuid != null &&
            res.data.data[0].clientUuid != ""
          ) {
            let Uuid = res.data.data[0].clientUuid;
            this.doloadlxrList(Uuid);
          }
        }
      });
    },
    //商机添加
    busClickCreate() {
      this.busEditModel.mobile.userName = this.$store.state.user.userName;
      this.busEditModel.mobile.userGuid = this.$store.state.user.userGuid;
      this.busEditModel.mobile.contactBusName = this.lxrcunlist;
      console.log(this.busEditModel.mobile.contactBusName);
      if (this.busEditModel.mobile.businessName == "") {
        this.$Message.warning("请填写商机名称！");
        return;
      }
      let reg = /^[^\s]+$/;
      if (!reg.test(this.busEditModel.mobile.businessName)) {
        this.$Message.warning("商机名称不合法");
        return;
      }
      if (this.busEditModel.mobile.clientUuid == "") {
        this.$Message.warning("请选择客户");
        return;
      }
      if (this.busEditModel.mobile.salesAmount != "") {
        let reg2 = /^\d+$|^\d+[.]?\d+$/;
        if (!reg2.test(this.busEditModel.mobile.salesAmount)) {
          this.$Message.warning("金额输入不合法");
          return;
        }
      }

      let data = this.busEditModel.mobile;
      console.log(data);
      BusAdd(data).then((res) => {
        if (res.data.code == 200) {
          this.$Message.success("商机添加成功");
          this.busEditModel.opened = false;

          this.loadBusList();
        } else {
          this.$Message.warning("商机添加失败");
        }
      });
    },
    //商机修改保存
    busClickEdit() {
      if (this.busEditModel.mobile.businessName == "") {
        this.$Message.warning("请填写商机名称");
        return;
      }
      let reg = /^[^\s]+$/;
      if (!reg.test(this.busEditModel.mobile.businessName)) {
        this.$Message.warning("商机名称不合法");
        return;
      }
      if (this.busEditModel.mobile.clientUuid == "") {
        this.$Message.warning("请选择所属客户");
        return;
      }
      if (this.busEditModel.mobile.salesAmount != "") {
        let reg2 = /^\d+$|^\d+[.]?\d+$/;
        if (!reg2.test(this.busEditModel.mobile.salesAmount)) {
          this.$Message.warning("金额输入不合法");
          return;
        }
      }
      this.busEditModel.mobile.userName = this.$store.state.user.userName;
      this.busEditModel.mobile.userGuid = this.$store.state.user.userGuid;
      let data = this.busEditModel.mobile;
      //console.log(data);
      BusEdit(data).then((res) => {
        if (res.data.code == 200) {
          this.$Message.success("商机修改成功");
          this.busEditModel.opened = false;
          this.loadBusList();
        } else {
          this.$Message.warning("商机修改失败");
        }
      });
    },
    busClick() {
      if (this.busEditModel.mode === "create") {
        this.busClickCreate();
      }
      if (this.busEditModel.mode === "edit") {
        this.busClickEdit();
      }
    },
    BushandleSelect(selection, row) {},
    //多选
    BushandleSelectionChange(selection) {
      this.formModel4.selection = selection;
    },
    BushandleSelect1(selection, row) {},
    //多选
    BushandleSelectionChange1(selection) {
      this.formModelLog.selection = selection;
    },
    //商机右上边删除按钮
    handleBatchCommand(command) {
      if (!this.selectRusRowsId || this.selectRusRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    //商机右上边删除
    doBatchCommand(command) {
      Batch({
        command: command,
        ids: this.selectRusRowsId.join(","),
        userName: this.$store.state.user.userName,
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadBusList();
          this.formModel4.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //翻页
    handlePageChanged2(page) {
      this.formModel4.buslist.query.currentPage = page;
      this.loadListAddress();
    },
    //显示条数改变
    handlePageSizeChanged2(pageSize) {
      this.formModel4.buslist.query.pageSize = pageSize;
      this.loadListAddress();
    },

    doCancel_xxx() {
      this.formModelLog.opened = false;
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
    rowClsRender_xxx(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },

    //---------------第二个模型  FIle-------------------
    rowClsRenderFile(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },
    loadFileList() {
      getFileList(this.stores.file.query).then((res) => {
        console.log(res);
        this.stores.file.data = res.data.data;
        this.stores.file.query.totalCount = res.data.totalCount;
      });
    },
    handleFileRefresh() {
      this.loadFileList();
    },
    handleFileSelect(selection, row) {},
    handleFileSelectionChange(selection) {
      this.busModelFile.selection = selection;
    },
    handleFilePageChanged(page) {
      this.stores.file.query.currentPage = page;
      this.loadFileList();
    },
    handleFilePageSizeChanged(pageSize) {
      this.stores.file.query.pageSize = pageSize;
      this.loadFileList();
    },
    handleSearchFile() {
      this.loadFileList();
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteFile(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadFileList();
          this.busModelFile.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleFileBatchCommand(command) {
      if (!this.selectFileRowsId || this.selectFileRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doFileBatchCommand(command);
        },
      });
    },
    doFileBatchCommand(command) {
      batchCommandFile({
        command: command,
        ids: this.selectFileRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadFileList();
          this.busModelFile.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleFileSortChange(column) {
      this.stores.file.query.sort.direction = column.order;
      this.stores.file.query.sort.field = column.key;
      this.loadFileList();
    },
    handleResetFormFile() {
      // this.$refs["formFile"].resetFields();
      this.busModelFile.fields.fileUrl = "";
      this.busModelFile.fields.fileName = "";
      this.busModelFile.fields.fileInfo = "";
    },
    handleOpenFormWindowFile() {
      this.busModelFile.formopened = true;
    },
    handleCloseFormWindowFile() {
      this.busModelFile.formopened = false;
    },
    handleSwitchFormModeToCreateFile() {
      this.busModelFile.mode = "create";
    },
    handleSwitchFormModeToEditFile() {
      this.busModelFile.mode = "edit";
      this.handleOpenFormWindowFile();
    },

    handleFileShowCreateWindow() {
      this.busModelFile.dFileName = "";
      this.$refs.upload.clearFiles();
      this.handleSwitchFormModeToCreateFile();
      this.handleOpenFormWindowFile();
      this.handleResetFormFile();
    },
    handleFileEdit(row) {
      this.busModelFile.dFileName = "";
      this.$refs.upload.fileList = [
        { name: "", status: "finished", showProgress: false },
      ];
      this.handleSwitchFormModeToEditFile();
      this.handleResetFormFile();
      this.doLoadFile(row.busDocumentsUuid);
    },
    doLoadFile(busDocumentsUuid) {
      loadFile({ guid: busDocumentsUuid }).then((res) => {
        this.busModelFile.fields = res.data.data;
        this.$refs.upload.fileList[0].name = this.busModelFile.fields.fileUrl.split(
          "\\"
        )[1];
        this.busModelFile.dFileName = res.data.data.path;
        this.url =
          config.baseUrl.dev +
          this.busModelFile.fields.fileUrl.replace("\\", "/");
      });
    },

    validateFileForm() {
      let _valid = false;
      this.$refs["formFile"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    handleSubmitFile() {
      console.log(this.stores.file.query.businessUuid);
      this.busModelFile.fields.businessUuid = this.stores.file.query.businessUuid;
      if (
        this.busModelFile.fields.fileUrl == null ||
        this.busModelFile.fields.fileUrl == ""
      ) {
        this.$Message.warning("请上传文件");
        return;
      }
      let valid = this.validateFileForm();

      //console.log(valid);
      //console.log(this.formModel.fields);
      if (valid) {
        if (this.busModelFile.mode === "create") {
          this.doCreateFile();
        }
        if (this.busModelFile.mode === "edit") {
          this.doEditFile();
        }
      }
    },
    doCreateFile() {
      console.log(this.busModelFile.fields);
      createFile(this.busModelFile.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindowFile();
          this.loadFileList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditFile() {
      console.log(this.busModelFile.fields);
      editFile(this.busModelFile.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindowFile();
          this.loadFileList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    handleFileDelete(row) {
      this.doDelete(row.busDocumentsUuid);
    },
    handleFileDown(row) {
      console.log(row);
      window.location.href =
        config.baseUrl.dev + row.fileUrl.replace("\\", "/");
    },

    showUpResult(e) {
      this.loadingStatus = false;
      this.updisabled = false;
      console.log(e);
      if (e.code == "200") {
        this.$Message.success(e.message);
        this.busModelFile.fields.fileUrl = e.data.dataPath;
        this.busModelFile.dFileName = e.data.path;
        console.log(
          (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
        console.log(this.busModelFile.dFileName);
      } else {
        this.$Message.warning(e.message);
      }
    },
    toUpResult() {
      console.log(this.$refs.upload.fileList);
      //console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
        // this.$refs.upload.clearFiles();
        // this.$refs.upload.push({})
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    deleteFile(e) {
      console.log(e);
      console.log(this.busModelFile.dFileName);
      // if (this.formModel.dFileName != null && this.formModel.dFileName != "") {
      //   deletetoFile({ filename: this.formModel.dFileName }).then(res => {
      //     console.log(res);
      //   });
      // }
    },
    download() {
      // console.log(this.url);
      window.location.href =
        config.baseUrl.dev +
        this.busModelFile.fields.fileUrl.replace("\\", "/");
    },

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
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl =
      config.baseUrl.dev + "api/v1/business/BusinessDocuments/UpLoad";
  },

  //页面加载时执行
  mounted() {
    this.loadDepartmentList1();
    this.loadListAddress();
    this.loaselect(); //穿梭框
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
