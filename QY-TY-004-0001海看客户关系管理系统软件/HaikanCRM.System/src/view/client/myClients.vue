<template>
  <div>
    <div>
      <section class="dnc-toolbar-wrap">
        <Row :gutter="16">
          <Col span="16">
            <Form inline @submit.native.prevent>
              <FormItem style="margin:15px 0">
                <Input
                  style="width: 300px;"
                  type="text"
                  search
                  :clearable="true"
                  v-model="stores.xxx1.query.kw"
                  placeholder="输入一级客户名称搜索..."
                  @on-search="sleet()"
                />
              </FormItem>
            </Form>
          </Col>
          <Col span="8" class="dnc-toolbar-btns" style="margin:15px 0;">
            <Button
              v-can="'create'"
              icon="md-create"
              type="primary"
              @click="handleShowCreateWindow"
              title="添加客户"
            >添加客户</Button>
          </Col>
        </Row>
      </section>
    </div>
    <vxe-table
      resizable
      :tree-config="{children: 'children'}"
      :data="tableData"
      :checkbox-config="{labelField: 'id', highlight: true}"
      @checkbox-change="selectChangeEvent"
    >
      <vxe-table-column type="checkbox" title="ID" width="120" tree-node></vxe-table-column>
      <vxe-table-column field="clientName" width="260" title="客户名称"></vxe-table-column>
      <vxe-table-column field="realName" width="130" title="客户经理"></vxe-table-column>
      <!-- <vxe-table-column field="cpName" width="400" title="联系人"></vxe-table-column> -->
      <vxe-table-column field="cpName" title="联系人">
        <template v-slot="{ row }">
          <span v-for="(name,index) in row.cpName" :key="index">
            <Button label="small" type="text" @click="handleDeletes(row,name)">{{name}}</Button>
          </span>
        </template>
      </vxe-table-column>
      <!-- <vxe-table-column field="clientBirthday" width="150" title="客户生日"></vxe-table-column> -->
      <vxe-table-column field="industryName" width="100" title="行业"></vxe-table-column>
      <vxe-table-column field="clientArea" width="100" title="地区"></vxe-table-column>
      <vxe-table-column field="statusName" width="100" title="状态"></vxe-table-column>
      <!-- <vxe-table-column field="systemUserName" title="参与人"></vxe-table-column> -->
      <vxe-table-column title="操作" width="140" >
        <template v-slot="{ row }">
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
          <Tooltip placement="top" content="分配客户" :delay="1000" :transfer="true">
            <Button
              v-can="'quanxian'"
              type="warning"
              size="small"
              shape="circle"
              icon="md-create"
              @click="naaccountmanager(row)"
            ></Button>
          </Tooltip>
        </template>
      </vxe-table-column>
    </vxe-table>
    <vxe-pager
      perfect
      :current-page.sync="stores.xxx1.query.currentPage"
      :page-size.sync="stores.xxx1.query.pageSize"
      :total="stores.xxx1.query.totalCount"
      :layouts="['PrevJump', 'PrevPage', 'Number', 'NextPage', 'NextJump', 'Sizes', 'FullJump', 'Total']"
      @page-change="vtpageChage"
    ></vxe-pager>

    <!-- 添加 -->
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="550"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="fromcreat" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <FormItem label="客户经理">
            <Input v-model="formModel.fields.userName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="24">
            <FormItem label-position="left" label="上级客户" prop="superName">
              <Select v-model="formModel.fields.superiorUuid" filterable>
                <Option
                  v-for="item in this.formModel.pickerkehu"
                  :value="item.value"
                  :key="item.value"
                >{{item.label}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="客户名称" prop="clientName">
              <Input v-model="formModel.fields.clientName" placeholder="客服名称" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="行业" prop="clientIndustry">
              <Select style="width:150px" v-model="formModel.fields.clientIndustry">
                <Option
                  v-for="item in stores.xxx1.sources.allIndustry"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>

        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="状态" prop="clientStatus">
              <Select style="width:150px" v-model="formModel.fields.clientStatus">
                <Option
                  v-for="item in stores.xxx1.sources.allStatus"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="类型" prop="clientType">
              <Select style="width:150px" v-model="formModel.fields.clientType">
                <Option
                  v-for="item in stores.xxx1.sources.allTypes"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="地区">
              <Select style="width:150px" v-model="formModel.fields.clientArea">
                <Option
                  v-for="item in stores.xxx1.cityList"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
          <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="客户等级">
              <Select style="width:150px" v-model="formModel.fields.superiorMenu">
                <Option
                  v-for="item in stores.xxx1.cityList"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
          <!-- <Col span="12">
            <FormItem label="客户生日" prop="birthTime">
              <DatePicker
                v-model="formModel.fields.birthTime"
                @on-change="formModel.fields.birthTime=$event"
                format="yyyy-MM-dd"
                type="date"
                placeholder="客户生日"
                style="width: 150px"
              ></DatePicker>
            </FormItem>
          </Col> -->
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="参与人" prop="systemUserName">
            <Input
              v-model="formModel.fields.systemUserName"
              @on-change="selectcanyu"
              style="width: 150px;"
              placeholder="请选择参与人"
              :readonly="true"
            />
            <i-button type="info" @click="selectcanyu(rowid)">选择参与人</i-button>
          </FormItem>
        </Row>-->
        <Row :gutter="16">
          <FormItem label="网站">
            <Input v-model="formModel.fields.clientWebsite" placeholder=" 网站" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="地址" prop="remarks">
            <Input v-model="formModel.fields.clientAddress" placeholder=" 地址" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="邮件">
            <Input v-model="formModel.fields.clientEmail" placeholder=" 邮件" />
          </FormItem>
        </Row>
        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="传真">
              <Input v-model="formModel.fields.clientFax" placeholder=" 传真" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="邮编">
              <Input v-model="formModel.fields.clientPostcode" placeholder=" 邮编" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input type="textarea" v-model="formModel.fields.remarks" placeholder="备注信息" />
          </FormItem>
        </Row>
      </Form>

      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitCreate">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>

    <Drawer
      title="修改客户信息"
      v-model="formModel2.opened"
      width="600"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModel2.fields"
        ref="formCarDispatchDetail"
        :rules="formModel2.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="客户名称" prop="clientName">
            <Input v-model="formModel2.fields.clientName" placeholder="客户名称" />
          </FormItem>
        </Row>
                <Row :gutter="16" style="margin-left: -16px;">
          <Col span="24">
            <FormItem label-position="left" label="上级客户" prop="superName">
              <Select v-model="formModel2.fields.superiorUuid" filterable>
                <Option
                  v-for="item in this.formModel.pickerkehu"
                  :value="item.value"
                  :key="item.value"
                >{{item.label}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>

        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="类型" prop="clientType">
              <Select style="width:150px" v-model="formModel2.fields.clientType">
                <Option
                  v-for="item in stores.xxx1.sources.allTypes"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="行业" prop="clientIndustry">
              <Select style="width:150px" v-model="formModel2.fields.clientIndustry">
                <Option
                  v-for="item in stores.xxx1.sources.allIndustry"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="地区" style="padding-top: 32px;">
              <Select style="width:150px" v-model="formModel2.fields.clientArea">
                <Option
                  v-for="item in stores.xxx1.cityList"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="状态" style="padding-top: 32px;" prop="clientStatus">
              <Select style="width:150px" v-model="formModel2.fields.clientStatus">
                <Option
                  v-for="item in stores.xxx1.sources.allStatus"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>

        <!-- <Row :gutter="16">
          <FormItem label="客户生日" prop="birthTime">
            <DatePicker
              v-model="formModel2.fields.birthTime"
              @on-change="formModel2.fields.birthTime=$event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="客户生日"
              style="width: 150px"
            ></DatePicker>
          </FormItem>
        </Row> -->
        <Row :gutter="16">
          <FormItem label="网站">
            <Input v-model="formModel2.fields.clientWebsite" placeholder=" 网站" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="邮件">
            <Input v-model="formModel2.fields.clientEmail" placeholder=" 邮件" />
          </FormItem>
        </Row>

        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="传真">
              <Input v-model="formModel2.fields.clientFax" placeholder=" 传真" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="邮编">
              <Input v-model="formModel2.fields.clientPostcode" placeholder=" 邮编" />
            </FormItem>
          </Col>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="参与人" prop="systemUserName">
            <Input
              v-model="formModel2.fields.systemUserName"
              @on-change="selectcanyu2"
              style="width: 150px;"
              placeholder="请选择参与人"
              :readonly="true"
            />
            <i-button type="info" @click="selectcanyu2(rowid)">选择参与人</i-button>
          </FormItem>
        </Row>-->
        <Row :gutter="16">
          <FormItem label="地址" prop="remarks">
            <Input v-model="formModel2.fields.clientAddress" placeholder=" 地址" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input type="textarea" v-model="formModel2.fields.remarks" placeholder="备注信息" />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="btnClickEdit">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel2.opened = false">取 消</Button>
      </div>
    </Drawer>

    <Drawer
      title="详情"
      v-model="formModel3.opened"
      width="1000"
      :mask-closable="false"
      :mask="false"
    >
      <template>
        <Tabs v-model="val" type="card" @on-click="doLoadData4()">
          <TabPane label="客户信息" name="name1">
            <Form :model="formModel3.fields" ref="formCarDispatch" label-position="top">
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="客户名称" prop="clientName">
                  <Input
                    v-model="formModel3.fields.clientName"
                    :readonly="true"
                    style="width:800px"
                  />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="上级客户">
                  <Input
                    v-model="formModel3.superName"
                    placeholder="暂无"
                    :readonly="true"
                    style="width:800px"
                  />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="客户生日">
                  <Input
                    v-model="formModel3.fields.clientBirthday"
                    :readonly="true"
                    style="width:800px"
                  />
                </FormItem>
              </Row>

              <Row :gutter="16" style="margin-left: 6px;">
                <Col span="10">
                  <FormItem label="类型">
                    <Select
                      style="width:150px"
                      v-model="formModel3.fields.clientType"
                      :disabled="true"
                    >
                      <Option
                        v-for="item in stores.xxx1.sources.allTypes"
                        :value="item.value"
                        :key="item.value"
                      >{{ item.label }}</Option>
                    </Select>
                  </FormItem>
                </Col>
                <Col span="10">
                  <FormItem label="行业">
                    <Select
                      style="width:150px"
                      v-model="formModel3.fields.clientIndustry"
                      :disabled="true"
                    >
                      <Option
                        v-for="item in stores.xxx1.sources.allIndustry"
                        :value="item.value"
                        :key="item.value"
                      >{{ item.label }}</Option>
                    </Select>
                  </FormItem>
                </Col>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <Col span="10">
                  <FormItem label="地区" style="padding-top: 32px;">
                    <Select
                      style="width:150px"
                      v-model="formModel3.fields.clientArea"
                      :disabled="true"
                    >
                      <Option
                        v-for="item in stores.xxx1.cityList"
                        :value="item.value"
                        :key="item.value"
                      >{{ item.label }}</Option>
                    </Select>
                  </FormItem>
                </Col>
                <Col span="10">
                  <FormItem label="状态" style="padding-top: 32px;">
                    <Select
                      style="width:150px"
                      v-model="formModel3.fields.clientStatus"
                      :disabled="true"
                    >
                      <Option
                        v-for="item in stores.xxx1.sources.allStatus"
                        :value="item.value"
                        :key="item.value"
                      >{{ item.label }}</Option>
                    </Select>
                  </FormItem>
                </Col>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="网站">
                  <Input
                    v-model="formModel3.fields.clientWebsite"
                    :readonly="true"
                    style="width:800px"
                  />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="邮件">
                  <Input
                    v-model="formModel3.fields.clientEmail"
                    :readonly="true"
                    style="width:800px"
                  />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 1px;">
                <Col span="12">
                  <FormItem label="传真">
                    <Input v-model="formModel3.fields.clientFax" :readonly="true" />
                  </FormItem>
                </Col>
                <Col span="12">
                  <FormItem label="邮编">
                    <Input v-model="formModel3.fields.clientPostcode" :readonly="true" />
                  </FormItem>
                </Col>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="地址" prop="remarks">
                  <Input
                    v-model="formModel3.fields.clientAddress"
                    :readonly="true"
                    style="width:800px"
                  />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="备注" prop="remarks">
                  <Input
                    type="textarea"
                    v-model="formModel3.fields.remarks"
                    :readonly="true"
                    style="width:800px"
                  />
                </FormItem>
              </Row>
            </Form>
          </TabPane>
          <TabPane label="联系人" name="name2">
            <dz-table
              class="carlist"
              :totalCount="formModel5.carlist.query.totalCount"
              :pageSize="formModel5.carlist.query.pageSize"
              @on-page-change="handlePageChanged3"
              @on-page-size-change="handlePageSizeChanged3"
            >
              <Table
                slot="table"
                ref="tables"
                :border="false"
                size="small"
                :highlight-row="true"
                :data="formModel5.carlist.data22"
                :columns="formModel5.carlist.columns"
                :row-class-name="rowClsRender_xxx"
              ></Table>
            </dz-table>
            <Divider orientation="left">新建联系人</Divider>
            <Form
              :model="formModel6.fields"
              ref="fromcrre"
              :rules="formModel6.rules"
              label-position="top"
            >
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label=" 联系人名称" prop="contactName">
                  <Input
                    v-model="formModel6.fields.contactName"
                    placeholder=" 联系人名称"
                    style="width:550px"
                  />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="职务">
                  <Input v-model="formModel6.fields.call" placeholder=" 职务" style="width:550px" />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="电话(多个电话请以逗号分隔)" prop="phone">
                  <Input v-model="formModel6.fields.phone" placeholder=" 电话号码" style="width:550px" />
                </FormItem>
              </Row>
              <Row :gutter="16" style="margin-left: 6px;">
                <FormItem label="邮箱" prop="email">
                  <Input v-model="formModel6.fields.email" placeholder=" 邮箱" style="width:550px" />
                </FormItem>
              </Row>
            </Form>
            <div class="demo-drawer-footer">
              <Button icon="md-checkmark-circle" type="primary" @click="handleSubmit22">添 加 联 系 人</Button>
            </div>
          </TabPane>
          <TabPane label="商机" name="name3">
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
                <div slot="searcher" style="text-align: right;">
                  <section class="dnc-toolbar-wrap">
                    <Row :gutter="16">
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
                      >添加商机信息</Button>
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
                  <template slot-scope="{row,index}" slot="state">
                    <span>{{CheckState(row.state)}}</span>
                  </template>
                  <template slot-scope="{ row, index }" slot="action">
                    <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
                      <Button
                        v-can="'busedit'"
                        type="primary"
                        size="small"
                        shape="circle"
                        icon="md-create"
                        @click="bushandleEdit(row)"
                      ></Button>
                    </Tooltip>
                  </template>
                </Table>
              </dz-table>
            </Form>
            <div class="demo-drawer-footer">
              <Button style="margin-left: 8px" icon="md-close" @click="doCancel_xxx">取 消</Button>
            </div>
          </TabPane>
        </Tabs>
      </template>
    </Drawer>

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
              placeholder=" 商机名称"
              style="width: 85%;"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="所属客户" prop="clientUuid">
            <Select v-model="busEditModel.mobile.clientUuid" style="width:400px" :disabled="true">
              <Option
                v-for="item in stores.xxx1.sources.clientName"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="相关联系人" prop="roleUuid" v-if="kaiguan">
            <Checkbox-group v-model="lxrcunlist" @on-change="testlxr">
              <Checkbox class="cbox" v-for="it in xglxrlist" :label="it.contactPersonUuid">
                <span>{{it.contactName}}</span>
              </Checkbox>
            </Checkbox-group>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12" style="margin-left:-8px;">
            <FormItem label="商机来源:" prop="businessSource">
              <Select style="width:150px" v-model="busEditModel.mobile.businessSource">
                <Option
                  v-for="item in busEditModel.pickerSource"
                  :value="item"
                  :key="item"
                >{{ item }}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="商机类型:" prop="businessType">
              <Select style="width:150px" v-model="busEditModel.mobile.businessType">
                <Option v-for="item in busEditModel.pickerType" :value="item" :key="item">{{ item }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="项目预算:" prop="salesAmount">
            <Input
              v-model="busEditModel.mobile.salesAmount"
              placeholder=" 项目预算"
              style="width: 85%;"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12" style="margin-left:-8px;">
            <FormItem label="商机赢率:" prop="winrate">
              <Select style="width:150px" v-model="busEditModel.mobile.winrate">
                <Option
                  v-for="item in busEditModel.pickerWinrate"
                  :value="item"
                  :key="item"
                >{{ item }}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="商机阶段:" prop="businessStage">
              <Select style="width:150px" v-model="busEditModel.mobile.businessStage">
                <Option
                  v-for="item in busEditModel.pickerStage"
                  :value="item"
                  :key="item"
                >{{ item }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="预计成交时间" prop="checkTime">
            <DatePicker
              v-model="busEditModel.mobile.checkTime"
              @on-change="busEditModel.mobile.checkTime=$event"
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
              @on-change="selectcanyu3"
              style="width: 150px;"
              placeholder="请选择参与人"
              :readonly="true"
            />
            <i-button type="info" @click="selectcanyu3(rowid)">选择参与人</i-button>
          </FormItem>
        </Row>-->
        <Row :gutter="16">
          <FormItem label="备注:">
            <Input
              type="textarea"
              v-model="busEditModel.mobile.remarks"
              placeholder="备注信息"
              style="width:85%;margin-left: 4%;"
            />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="busClick">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="busEditModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      title="查看商机信息"
      v-model="busViewModel.opened"
      width="600"
      :mask-closable="false"
      :mask="false"
    >
      <Form :model="busViewModel.query" ref="formCarDispatchDetail" label-position="left">
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
            <Input v-model="busViewModel.query.businessSource" :readonly="true" />
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
            <Input v-model="busViewModel.query.businessStage" :readonly="true" />
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
        <Button style="margin-left: 30px" icon="md-close" @click="busViewModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
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
    </Drawer>
    <Drawer
      title="选择参与人"
      v-model="formModelcanyu2.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModelcanyu2.fields" ref="formselect" label-position="top">
        <FormItem label="参与人" prop="participant">
          <Transfer
            :data="formModelcanyu2.fields.participantleft"
            :target-keys="formModelcanyu2.fields.participantright"
            :list-style="{width: '240px',height: '400px'}"
            :titles="['所有人员','选择的人员']"
            filterable
            @on-change="handleSchoolChange2"
          ></Transfer>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="formModelcanyu2.opened = false"
        >确 定</Button>
      </div>
    </Drawer>
    <Drawer
      title="选择参与人"
      v-model="formModelcanyu3.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModelcanyu3.fields" ref="formselect" label-position="top">
        <FormItem label="参与人" prop="participant">
          <Transfer
            :data="formModelcanyu3.fields.participantleft"
            :target-keys="formModelcanyu3.fields.participantright"
            :list-style="{width: '240px',height: '400px'}"
            :titles="['所有人员','选择的人员']"
            filterable
            @on-change="handleSchoolChange3"
          ></Transfer>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="formModelcanyu3.opened = false"
        >确 定</Button>
      </div>
    </Drawer>

    <!-- 分配客户 -->
    <Drawer
      title="分配客户"
      v-model="formModefenpei.opened"
      width="550"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form
        :model="formModefenpei.fields"
        ref="fromcreatfenpei"
        :rules="formModefenpei.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label-position="left" label="客户经理" prop="rowUuid">
            <Select v-model="formModefenpei.rowUuid" filterable>
              <Option
                v-for="item in this.formModefenpei.accountmanager"
                :value="item.value"
                :key="item.value"
              >{{item.label}}</Option>
            </Select>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="editmangerpower">分配客户</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModefenpei.opened = false">取 消</Button>
      </div>
    </Drawer>


    <Drawer
      title="联系人详情"
      v-model="formModeCP.opened"
      width="550"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form
        :model="formModeCP.fields"
        ref="fromCP"
        label-position="top"
      >
        <!-- <Row :gutter="16">
          <FormItem label="客户经理">
            <Input readonly v-model="formModeCP.fields.userName" :readonly="true" />
          </FormItem>
        </Row> -->
        <Row :gutter="16">
          <FormItem label="联系人姓名" prop="contactName">
            <Input readonly v-model="formModeCP.fields.contactName" placeholder="联系人姓名" />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="所属客户" prop="clientUuid">
            <Select disabled v-model="formModeCP.fields.clientUuid" style="width:400px" filterable>
              <Option
                v-for="item in stores.xxx1.sources.clientName"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16" style="margin-left: -16px;">
          <Col span="12">
            <FormItem label="职务" prop="call">
              <Input readonly v-model="formModeCP.fields.call" placeholder="职务" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="性别" prop="sex">
              <Select disabled v-model="formModeCP.fields.sex" style="width:150px" placeholder="性别">
                <Option
                  v-for="item in stores.xxx1.sexList"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="电话(如有多个以空格隔开)" prop="phone">
            <Input readonly v-model="formModeCP.fields.phone" placeholder="电话号码" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="邮箱" prop="email">
            <Input readonly v-model="formModeCP.fields.email" placeholder="邮箱" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="微信" prop="weChat">
            <Input readonly v-model="formModeCP.fields.weChat" placeholder="微信" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="QQ" prop="tencentQq">
            <Input readonly v-model="formModeCP.fields.tencentQq" placeholder="QQ" />
          </FormItem>
        </Row>
        
        <Row :gutter="16">
          <FormItem label="家庭地址" prop="familyAddress">
            <Input readonly v-model="formModeCP.fields.familyAddress" placeholder="家庭地址" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="办公地址" prop="officeAddress">
            <Input readonly v-model="formModeCP.fields.officeAddress" placeholder="办公地址" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input readonly type="textarea" v-model="formModeCP.fields.remarks" placeholder="备注信息" />
          </FormItem>
        </Row>
      </Form>
    </Drawer>
  </div>
</template>



	<script src="https://cdn.jsdelivr.net/npm/vue"></script>
	<script src="https://cdn.jsdelivr.net/npm/xe-utils"></script>
	<script src="https://cdn.jsdelivr.net/npm/vxe-table"></script>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  loadSimpleList,
} from "@/api/contact/contactPeople";
import {
  loadEditData2 //加载编辑
} from "@/api/contact/contactPeople";
import {
  getClientsList,
  getAllStatus,
  getAllTypes,
  getAllIndustry,
  create1,
  getTravel,
  editSave, //编辑保存
  loadEditData, //加载编辑
  loadEditData1, //加载商机
  editSaveBusi, //保存商机
  EditContactPerson,
  deleteCarDriver,
  binddataok,
  binddataok1,
  getcanyuren,
  BusView,
  BusEdit,
  gealluser,
  GetAllCustomer,
  geallmanager, //权限选择框
  BusAdd,
  Batch,
  loadMenuTree,
  SuploadMenuTree,
  getList
} from "@/api/customer/myClients";
import { getkehu, save_manger } from "@/api/contact/contactPeople";
export default {
  name: "myclients",
  components: {
    DzTable
  },
  data() {
    let globalvalidatePhone = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        let reg = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
        if (!reg.test(value)) {
          callback(new Error(" 电话号码不合法"));
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
          callback(new Error(" 名称不合法"));
        }
        callback();
      } else {
        callback(new Error("名称不能为空"));
      }
      callback();
    };
    return {
      tableData: [],
      commands: {
        delete: { name: "delete", title: "删除" }
      },
      panduanrowid: 0,
      rowid: "", //选择行的id
      val: "",
      usName: "",
      usGuid: "",
      clUuid: "",
      clNam: "",
      xglxrlist:[],
      lxrLiss:[],
      lxrLis: [],
      lxrlist: [],
      lxrcunlist: [],
      kaiguan:true,
      dio: true,
      modal1: false,
      //form保存参数
      formModeCP:{
        opened:false,
        fields:{
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
          userName: ""
        }

      },
      formModelcanyu: {
        opened: false,
        title: "选择人员",
        mode: "select",
        fields: {
          participantleft: [],
          participantright: []
        }
      },
      formModelcanyu2: {
        opened: false,
        title: "选择人员",
        mode: "select",
        fields: {
          participantleft: [],
          participantright: []
        }
      },
      formModelcanyu3: {
        opened: false,
        title: "选择人员",
        mode: "select",
        fields: {
          participantleft: [],
          participantright: []
        }
      },
      formModefenpei: {
        pickerkehu: [],
        accountmanager: [],
        selection: [],
        opened: false,
        rowUuid: "",
        clientUuid: "",
        mode: "",
        superName: "",
        fields: {
          // 具体实例保存参数
          clientUuid: "",
          clientName: "",
          clientManager: "",
          birthTime: "",
          clientStatus: "",
          clientPhone: "",
          clientEmail: "",
          clientFax: "",
          clientPostcode: "",
          clientType: "",
          clientIndustry: "",
          clientArea: "",
          clientWebsite: "",
          clientAddress: "",
          remarks: "",
          addTime: "",
          businessUUID: "",
          callLogUUID: "",
          status: "",
          contactName: "",
          title: "",
          call: "",
          phone: "",
          cellphone: "",
          email: "",
          userGuid: "",
          userName: "",
          systemUserName: "",
          systemUserUuid: "",
          superiorUuid: ""
        },
        rules: {}
      },

      formModel: {
        pickerkehu: [],
        accountmanager: [],
        Prioritylist: {
          picker: [
            {
              clientUuid: "",
              clientName: ""
            }
          ],
          index2: -1
        },

        selection: [],
        opened: false,
        openeds: false,
        rowUuid: "",
        clientUuid: "",
        mode: "",
        superName: "",
        fields: {
          // 具体实例保存参数
          clientUuid: "",
          clientName: "",
          clientManager: "",
          birthTime: "",
          clientStatus: "",
          clientPhone: "",
          clientEmail: "",
          clientFax: "",
          clientPostcode: "",
          clientType: "",
          clientIndustry: "",
          clientArea: "",
          clientWebsite: "",
          clientAddress: "",
          remarks: "",
          addTime: "",
          businessUUID: "",
          callLogUUID: "",
          status: "",
          contactName: "",
          title: "",
          call: "",
          phone: "",
          cellphone: "",
          email: "",
          userGuid: "",
          userName: "",
          systemUserName: "",
          systemUserUuid: "",
          superiorUuid: ""
        },
        rules: {
          clientName: [
            { type: "string", required: true, validator: validateName }
          ],
          // clientStatus: [
          //   { type: "string", required: true, message: "请选择状态" }
          // ],
          // clientType: [
          //   { type: "string", required: true, message: "请选择类型" }
          // ],
          clientIndustry: [
            { type: "string", required: true, message: "请选择行业" }
          ]
        }
      },
      sources: {
        formSource: {
          menuTree: {
            data: []
          }
        },
        formSource1: {
          menuTree: {
            data: []
          }
        }
      },
      formModel2: {
        selection: [],
        pickerkehu: [],
        accountmanager: [],
        opened: false,
        mode: "",
        superName: "",
        fields: {
          // 具体实例保存参数
          clientName: "",
          clientManager: "",
          clientStatus: "",
          birthTime: "",
          clientPhone: "",
          clientEmail: "",
          clientFax: "",
          clientPostcode: "",
          clientType: "",
          clientIndustry: "",
          clientArea: "",
          clientWebsite: "",
          clientAddress: "",
          remarks: "",
          addTime: "",
          businessUuid: "",
          callLogUuid: "",
          status: "",
          contactName: "",
          title: "",
          call: "",
          phone: "",
          cellphone: "",
          email: "",
          userGuid: "",
          userName: "",
          systemUserName: "",
          systemUserUuid: "",
          superiorUuid: ""
        },
        rules: {
          clientName: [
            { type: "string", required: true, validator: validateName }
          ],
          // clientStatus: [
          //   { type: "string", required: true, message: "请选择状态" }
          // ],
          // clientType: [
          //   { type: "string", required: true, message: "请选择类型" }
          // ],
          clientIndustry: [
            { type: "string", required: true, message: "请选择行业" }
          ]
        }
      },
      formModel3: {
        selection: [],
        opened: false,
        mode: "",
        superName: "",
        fields: {
          // 具体实例保存参数
          clientName: "",
          clientManager: "",
          clientStatus: "",
          clientBirthday: "",
          clientPhone: "",
          clientEmail: "",
          clientFax: "",
          clientPostcode: "",
          clientType: "",
          clientIndustry: "",
          clientArea: "",
          clientWebsite: "",
          clientAddress: "",
          remarks: "",
          addTime: "",
          businessUUID: "",
          callLogUUID: "",
          status: "",
          contactName: "",
          title: "",
          call: "",
          phone: "",
          cellphone: "",
          email: "",
          userGuid: "",
          userName: "",
          superiorUuid: "",
          systemUserName: ""
        }
      },

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
            kw: "",
            ClientUUID: "",
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          columns: [
            { type: "selection", width: 35, key: "businessUuid" },
            { title: "商机名称", key: "businessName", minWidth: 100 },
            { title: "客户名称", key: "clientName", minWidth: 150 },
            { title: "项目预算", key: "salesAmount", width: 100 },
            { title: "预计成交时间", key: "checkTime", width: 130 },
            { title: "商机阶段", key: "businessStage", width: 75 },
            // { title: "参与人", key: "systemUserName", minWidth: 100 },
            { title: "相关联系人", key: "systemUserName", minWidth: 100 },
            {
              title: "操作",
              align: "center",
              fixed: "right",
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        },
        //自定义样式
        styles: {}
      },
      busEditModel: {
        opened: false,
        title: "修改商机",
        mode: "edit",
        Prioritylist: {
          picker: [
            {
              clientUuid: "",
              clientName: ""
            }
          ],
          index2: -1
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
          contactBusName: ""
        },
        rules: {
          businessName: [
            { type: "string", required: true, message: " 请输入名称" }
          ],
          clientUuid: [
            { type: "string", required: true, message: "请选择客户" }
          ]
        }
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
          clientBirthday: "",
          systemUserName: ""
        }
      },
      formModel5: {
        opened: false,
        title: "历史轨迹",
        carlist: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDelete: 0,
            status: -1,
            kw: "",
            kw1: "",
            kw2: [],
            carUuid: "",
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          columns: [
            { title: "联系人名称", key: "contactName",width:100 },
            { title: "客户名称", key: "clientName", },
            { title: "职务", key: "call",width:100 },
            { title: "电话", key: "phone" },
            { title: "邮箱", key: "email" }
          ],
          data22: []
        }
      },
      formModel6: {
        selection: [],
        opened: false,
        mode: "",
        fields: {
          // 具体实例保存参数
          contactName: "",
          title: "",
          call: "",
          phone: "",
          email: "",
          clientUuid: "",
          userGuid: "",
          userName: ""
        },
        rules: {
          contactName: [
            { type: "string", required: true, validator: validateName }
          ]
        }
      },
      //...
      //用于提交及一些保持性数据
      stores: {
        //该实例对象相关数据
        xxx1: {
          clientName: [
            {
              value: "",
              label: ""
            }
          ],
          cityList: [
            { value: "河北省", label: "河北省" },
            { value: "山西省", label: "山西省" },
            { value: "辽宁省", label: "辽宁省" },
            { value: "吉林省", label: "吉林省" },
            { value: "黑龙江省", label: "黑龙江省" },
            { value: "江苏省", label: "江苏省" },
            { value: "浙江省", label: "浙江省" },
            { value: "安徽省", label: "安徽省" },
            { value: "福建省", label: "福建省" },
            { value: "江西省", label: "江西省" },
            { value: "山东省", label: "山东省" },
            { value: "河南省", label: "河南省" },
            { value: "湖北省", label: "湖北省" },
            { value: "湖南省", label: "湖南省" },
            { value: "广东省", label: "广东省" },
            { value: "海南省", label: "海南省" },
            { value: "四川省", label: "四川省" },
            { value: "贵州省", label: "贵州省" },
            { value: "云南省", label: "云南省" },
            { value: "陕西省", label: "陕西省" },
            { value: "甘肃省", label: "甘肃省" },
            { value: "青海省", label: "青海省" },
            { value: "台湾省", label: "台湾省" },
            { value: "内蒙古自治区", label: "内蒙古自治区" },
            { value: "广西壮族自治区", label: "广西壮族自治区" },
            { value: "西藏自治区", label: "西藏自治区" },
            { value: "宁夏回族自治区", label: "宁夏回族自治区" },
            { value: "新疆维吾尔自治区", label: "新疆维吾尔自治区" },
            { value: "北京市", label: "北京市" },
            { value: "天津市", label: "天津市" },
            { value: "上海市", label: "上海市" },
            { value: "重庆市", label: "重庆市" },
            { value: "香港特别行政区", label: "香港特别行政区" },
            { value: "澳门特别行政区", label: "澳门特别行政区" }
          ],
          grade:[
            {value:"A",label: "A"},
            {value:"B",label: "B"},
            {value:"C",label: "C"},
            {value:"D",label: "D"},
          ],
          sexList: [
            {
              value: "1",
              label: "男"
            },
            {
              value: "2",
              label: "女"
            },
            {
              value: "3",
              label: "未知"
            }
          ],
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
            // kw2: "",
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
          sources: {
            feedback: [
              {
                // value: "thrdayout",
                value: "3",
                label: "3天未联系的客户"
              },
              {
                value: "7",
                // value: "weekout",
                label: "1周未联系的客户"
              },
              {
                value: "30",
                // value: "monthout",
                label: "1个月未联系的客户"
              },
              {
                value: "weekin",
                // value: "weekin",
                label: "本周内联系的客户"
              },
              {
                value: "monthin",
                // value: "monthin",
                label: "本月内联系的客户"
              },
              {
                value: "eveout",
                // value: "eveout",
                label: "无联系的客户"
              }
            ],
            allStatus: [],
            allTypes: [],
            allIndustry: []
          },
          //table的列项名称
          columns: [
            //选择框
            { type: "selection", width: 50, key: "handle" },
            { title: "客户名称", key: "clientName" },
            //可进行逻辑处理的条件规则显示项,以slot绑定
            // { title: "客户经理", key: "xxx", slot: "state" },
            { title: "客户经理", key: "realName" },
            { title: "状态", key: "statusName" },
            //{ title: "参与人", key: "systemUserName" },
            {
              title: "上下级客户",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "state"
            },
            //操作按钮显示样式
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action"
            }
          ]
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
      if ((this.formModel.mode = "create")) {
        return "添加客户信息";
      }
      if ((this.formModel.mode = "edit")) {
        return "编辑客户信息";
      }
    },
    formTitle1() {
      if (this.busEditModel.mode === "create") {
        return "添加商机信息";
      }
      if (this.busEditModel.mode === "edit") {
        return "编辑商机信息";
      }
      return "";
    },
    //删除
    selectRusRowsId() {
      return this.formModel4.selection.map(x => x.businessUuid);
    },
    //...
    //获取表格选择的行id
    selectRowsId() {
      //返回具体选择项的uuid
      return this.formModel.selection.map(x => x.xxxUuid);
    }
    //...
  },

  created() {
    getList(this.stores.xxx1.query).then(res => {
      console.log("返回的数据");
      console.log(res);
      this.tableData = res.data.obj;
      for (let k = 0; k < res.data.obj.length; k++) {
          this.lxrLis[k] = res.data.obj[k].cpName;
        if (res.data.obj[k].cpName.length > 0) { 
         //console.log(res.data.obj[k].cpName);         
          for (let j = 0; j < res.data.obj[k].cpName.length; j++) {
            this.lxrLiss[j] = res.data.obj[k].cpName[j];
          }
        }
      }
      //console.log(this.lxrLis);
      console.log(this.lxrLiss);
      this.stores.xxx1.query.totalCount = res.data.totalCount;

      //this.lxrLis = res.data.cpName.split(",");
      //console.log(this.lxrLis);
    });
  },
  //方法集合
  methods: {
    dogetList() {
      getList(this.stores.xxx1.query).then(res => {
        console.log("返回的数据");
        console.log(res);
        this.tableData = res.data.obj;
        this.stores.xxx1.query.totalCount = res.data.totalCount;

        // this.lxrLis = res.data.cpName[0].split(",");
        // console.log(this.lxrLis);
      });
    },
    sleet() {
      this.usName = this.$store.state.user.userName;
      this.usGuid = this.$store.state.user.userGuid;
      this.stores.xxx1.query.kw1 = this.$store.state.user.userGuid;
      getList(this.stores.xxx1.query).then(res => {
        console.log("返回的数据");
        console.log(res);
        this.tableData = res.data.obj;
      });
    },
    vtpageChage(e) {
      console.log(e);
      console.log(this.stores.xxx1.query);
      getList(this.stores.xxx1.query).then(res => {
      console.log("返回的数据");
      console.log(res);
      this.tableData = res.data.obj;
      this.stores.xxx1.query.totalCount=res.data.totalCount;
      // this.lxrLis = res.data.cpName[0].split(",");
      // console.log(this.lxrLis);
    });
    },
    handleDeletes(e,f){
      this.$refs["fromCP"].resetFields();
      console.log(e);
      console.log(f);
      let guid = e.clientuuid;
      let name = f;
      this.formModeCP.opened=true;
      loadEditData2({guid,name}).then(res=>{
        this.formModeCP.fields=res.data.data;
      });
    },

    selectChangeEvent({ records }) {
      console.info(`勾选${records.length}个树形节点`, records);

      var cliuuid = "";
      for (let i = 0; i < records.length; i++) {
        cliuuid += records[i].clientuuid + ",";
      }
      console.info(`勾选${records.length}个树形节点uuid`, cliuuid);
    },
    //编辑
    handleEdit22(row) {
      console.log("点击的行");
      console.log(row);
      // this.handleOpenFormWindow1();
      // this.handleSwitchFormModeToEdit();
      // this.handleResetFormDispatch1();
      // this.rowid = row.id; //保存选择行的id
      // this.formModelcanyu.fields.participantright = [];
      // this.doLoadEditData(row.clientUuid);
    },
    //详情
    showInfo_xxx22(row) {
      console.log("点击的行");
      console.log(row);
    },

    //穿梭框
    loaselect() {
      getcanyuren().then(res => {
        this.formModelcanyu.fields.participantleft = res.data.data;
        this.formModelcanyu2.fields.participantleft = res.data.data;
        this.formModelcanyu3.fields.participantleft = res.data.data;
      });
    },
    //打开选择参与人窗口
    selectcanyu(rowid) {
      console.log(rowid);
      this.formModelcanyu.opened = true;
      //修改
      if (rowid != "") {
        binddataok({ id: rowid }).then(res => {
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
    //打开选择参与人窗口
    selectcanyu2(rowid) {
      console.log(rowid);
      this.formModelcanyu2.opened = true;
      //修改
      if (rowid != "") {
        binddataok({ id: rowid }).then(res => {
          console.log(res);
          if (this.panduanrowid != rowid) {
            //判断是否是本行的数据
            //不是本行数据
            this.formModelcanyu2.fields.participantright = []; //清除上次选择的人员
          }
          console.log(369);
          //如果没有选择人员则显示数据库中的内容（表示选择了新行）
          if (this.formModelcanyu2.fields.participantright.length == 0) {
            if (res.data.data.length > 0) {
              for (let i = 0; i < res.data.data.length; i++) {
                //显示数据库保存的人员
                this.formModelcanyu2.fields.participantright = this.formModelcanyu2.fields.participantright.concat(
                  res.data.data[i].key
                );
              }
            }
          }
          this.panduanrowid = rowid; //将本行的id保存
        });
      }
    },
    //打开选择参与人窗口
    selectcanyu3(rowid) {
      console.log(rowid);
      this.formModelcanyu3.opened = true;
      //修改
      if (rowid != "") {
        binddataok1({ id: rowid }).then(res => {
          console.log(res);
          if (this.panduanrowid != rowid) {
            //判断是否是本行的数据
            //不是本行数据
            this.formModelcanyu3.fields.participantright = []; //清除上次选择的人员
          }
          console.log(369);
          //如果没有选择人员则显示数据库中的内容（表示选择了新行）
          if (this.formModelcanyu3.fields.participantright.length == 0) {
            if (res.data.data.length > 0) {
              for (let i = 0; i < res.data.data.length; i++) {
                //显示数据库保存的人员
                this.formModelcanyu3.fields.participantright = this.formModelcanyu3.fields.participantright.concat(
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
      this.formModel.fields.systemUserName = this.formModel.fields.systemUserUuid =
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
            this.formModel.fields.systemUserName +=
              this.formModelcanyu.fields.participantleft[j].label + ",";
            this.formModel.fields.systemUserUuid +=
              this.formModelcanyu.fields.participantleft[j].key + ",";
          }
        }
      }
      console.log(this.formModelcanyu.fields.participantright);
    },
    //选择参与人
    handleSchoolChange2(newTargetKeys, direction, moveKeys) {
      this.formModelcanyu2.fields.participantright = newTargetKeys;
      this.formModel2.fields.systemUserName = this.formModel2.fields.systemUserUuid =
        ""; //每次选择则参与人都重新赋值
      for (
        let i = 0;
        i < this.formModelcanyu2.fields.participantright.length;
        i++
      ) {
        for (
          let j = 0;
          j < this.formModelcanyu2.fields.participantleft.length;
          j++
        ) {
          if (
            this.formModelcanyu2.fields.participantright[i] ===
            this.formModelcanyu2.fields.participantleft[j].key
          ) {
            this.formModel2.fields.systemUserName +=
              this.formModelcanyu2.fields.participantleft[j].label + ",";
            this.formModel2.fields.systemUserUuid +=
              this.formModelcanyu2.fields.participantleft[j].key + ",";
          }
        }
      }
    },
    //选择参与人
    handleSchoolChange3(newTargetKeys, direction, moveKeys) {
      this.formModelcanyu3.fields.participantright = newTargetKeys;
      this.busEditModel.mobile.systemUserName = this.busEditModel.mobile.systemUserUuid =
        ""; //每次选择则参与人都重新赋值
      for (
        let i = 0;
        i < this.formModelcanyu3.fields.participantright.length;
        i++
      ) {
        for (
          let j = 0;
          j < this.formModelcanyu3.fields.participantleft.length;
          j++
        ) {
          if (
            this.formModelcanyu3.fields.participantright[i] ===
            this.formModelcanyu3.fields.participantleft[j].key
          ) {
            this.busEditModel.mobile.systemUserName +=
              this.formModelcanyu3.fields.participantleft[j].label + ",";
            this.busEditModel.mobile.systemUserUuid +=
              this.formModelcanyu3.fields.participantleft[j].key + ",";
          }
        }
      }
    },

    btnClickEdit() {
      this.doEditDispatch();
    },
    //编辑（保存）
    doEditDispatch() {
      let reg = /^[^\s]+$/;
      if (!reg.test(this.formModel2.fields.clientName)) {
        this.$Message.warning(" 无效的名称");
        return;
      }
      // var regn = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
      // if (!regn.test(this.formModel2.fields.clientPhone)) {
      //   this.$Message.warning("电话不合法");
      //   return;
      // }
      this.formModel2.fields.userGuid = this.$store.state.user.userGuid;
      this.formModel2.fields.userName = this.$store.state.user.userName;
      console.log(this.formModel2.fields);
      editSave(this.formModel2.fields).then(res => {
        // console.log(this.formModel.fields)
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow1();
          this.sleet();
          //this.handleRefreshMenuTreeData();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    //右边编辑
    handleEdit(row) {
      console.log(row.clientuuid);
      //alert(row.id);
      this.handleOpenFormWindow1();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch1();
      this.rowid = row.id; //保存选择行的id
      this.formModelcanyu.fields.participantright = [];
      this.doLoadEditData(row.clientuuid);
    },
    handleSwitchFormModeToEdit() {
      this.formModel2.mode = "edit";
    },
    doLoadEditData(clientUuid) {
      loadEditData({ guid: clientUuid }).then(res => {
        console.log(res.data);
        //this.stores.contactPerson.sources.clientName = res.data.data.clientName;
        this.formModel2.fields = res.data.data.query;
        this.formModel2.superName = res.data.data.supname;
        this.formModel2.fields.birthTime = res.data.data.query.clientBirthday;
      });
    },

    //清空
    handleResetFormDispatch1() {
      this.$refs["formCarDispatchDetail"].resetFields();
    },

    //打开窗口
    handleOpenFormWindow1() {
      this.formModel2.opened = true;
    },
    //自动关闭窗口
    handleCloseFormWindow1() {
      this.formModel2.opened = false;
    },

    //详情
    showInfo_xxx(row) {
      this.clUuid = row.clientuuid;
      this.clNam = row.clientName;
      this.formModel6.fields.clientUuid = row.clientuuid;
      this.val = "name1";
      this.handleOpenFormWindow();
      this.handleResetFormDispatch2();
      this.doLoadEditData3(row.clientuuid);
    },
    doLoadEditData3(clientUuid) {
      loadEditData({ guid: clientUuid }).then(res => {
        console.log(res.data);
        //this.stores.contactPerson.sources.clientName = res.data.data.clientName;
        this.formModel3.fields = res.data.data.query;
        this.formModel3.superName = res.data.data.supname;
      });
      this.doloadlxrList(clientUuid);
    },

    handleResetFormDispatch2() {
      this.$refs["formCarDispatch"].resetFields();
    },
    //查看详情打开窗口
    handleOpenFormWindow() {
      this.formModel3.opened = true;
    },
    doLoadData4() {
      let clientUuid = this.clUuid;
      let val = this.val;
      let clNam = this.clNam;
      let usName = this.$store.state.user.userName;
      if (val == "name1") {
        console.log(clientUuid);
        loadEditData({ guid: clientUuid }).then(res => {
          this.formModel3.fields = res.data.data;
          this.formModel3.superName = res.data.data.supname;
        });
      } else if (val == "name2") {
        this.formModel5.carlist.query.clientUuid = clientUuid;
        getTravel(this.formModel5.carlist.query).then(res => {
          console.log(res);
          this.formModel5.carlist.data22 = res.data.data;
          this.formModel5.carlist.query.totalCount = res.data.totalCount;
          this.formModel5.opened = true;
        });
        console.log(this.formModel5);
      } else if (val == "name3") {
        this.handleResetFormh();
        this.formModel4.buslist.query.ClientUUID = clientUuid;
        this.doloadlxrList(this.formModel4.buslist.query.ClientUUID );
        loadEditData1(this.formModel4.buslist.query).then(res => {
          this.formModel4.buslist.data = res.data.data;
          this.formModel4.buslist.query.totalCount = res.data.totalCount;
        });
      }
    },
    //商机刷新
    loadBusList() {
      this.val = "name3";
      this.doLoadData4();
    },
    //商机添加
    handleBusCreateWindow() {
      this.lxrlist = [];
      this.lxrcunlist = [];
      this.kaiguan=true;
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
      BusView(data).then(res => {
        if (res.data.code == 200) {
          this.busViewModel.query = res.data.data[0];
        }
      });
    },
    BusFormModeToEdit() {
      this.busEditModel.mode = "edit";
    },
    //商机修改
    bushandleEdit(row) {
      this.kaiguan = false;
      this.busEditModel.opened = true;
      this.rowid = row.id; //保存选择行的id
      this.formModelcanyu3.fields.participantright = [];
      this.BusFormModeToEdit();
      this.$refs["BusDispatchDetail"].resetFields();
      this.busEditModel.mobile.businessUuid = row.businessUuid;
      gealluser().then(res => {
        this.busEditModel.Prioritylist.picker = res.data.data;
      });
      for (let i = 0; i < 101; i++) {
        this.busEditModel.pickerWinrate[i] = i + "%";
      }
      let data = row.businessUuid;
      BusView(data).then(res => {
        if (res.data.code == 200) {
          this.busEditModel.mobile = res.data.data[0];
          console.log(this.busEditModel.mobile);
        }
      });
    },
    //商机添加
    busClickCreate() {
      this.busEditModel.mobile.userName = this.$store.state.user.userName;
      this.busEditModel.mobile.userGuid = this.$store.state.user.userGuid;
      this.busEditModel.mobile.contactBusName = this.lxrcunlist;
      if (this.busEditModel.mobile.businessName == "") {
        this.$Message.warning("请填写商机名称！");
        return;
      }
      let reg = /^[^\s]+$/;
      if (!reg.test(this.busEditModel.mobile.businessName)) {
        this.$Message.warning("商机名称不合法");
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
      BusAdd(data).then(res => {
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
      this.busEditModel.mobile.userName = this.$store.state.user.userName;
      this.busEditModel.mobile.userGuid = this.$store.state.user.userGuid;
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
        this.$Message.warning("请选择所属客户！");
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
      BusEdit(data).then(res => {
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
        }
      });
    },
    //商机右上边删除
    doBatchCommand(command) {
      Batch({
        command: command,
        ids: this.selectRusRowsId.join(","),
        userName: this.$store.state.user.userName
      }).then(res => {
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
    handleSubmit22() {
      var regn = /^[a-zA-Z\u4e00-\u9fa5]+$/;
      if (!regn.test(this.formModel6.fields.contactName)) {
        this.$Message.warning("联系人名称不合法");
        return;
      }
      //console.log(this.formModel6.fields);
      let contactName = this.formModel6.fields.contactName;
      if (contactName != "") {
        this.doEditDispatch22();
      } else {
        this.$Message.warning("联系人名称不能为空");
      }
    },
    handleResetFormDispatch444() {
      this.$refs["fromcrre"].resetFields();
    },
    doEditDispatch22() {
      this.formModel6.fields.userGuid = this.$store.state.user.userGuid;
      this.formModel6.fields.userName = this.$store.state.user.userName;
      //console.log(this.formModel6.fields);
      EditContactPerson(this.formModel6.fields).then(res => {
        // console.log(this.formModel.fields)
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleResetFormDispatch444();
          this.val = "name2";
          this.doLoadData4();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    //清空
    handleResetFormh() {
      this.$refs["fromcreat"].resetFields();
    },
    //表格行样式
    rowClsRender_xxx(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },
    loadOtherInfo() {
      getAllStatus().then(res => {
        this.stores.xxx1.sources.allStatus = res.data.data;
      });
      getAllTypes().then(res => {
        this.stores.xxx1.sources.allTypes = res.data.data;
      });
      getAllIndustry().then(res => {
        this.stores.xxx1.sources.allIndustry = res.data.data;
      });
    },
    //加载表格数据
    sleet22() {
      //console.log(this.$store.state.user);
      this.usName = this.$store.state.user.userName;
      this.stores.xxx1.query.kw1 = this.$store.state.user.userGuid;
      getClientsList(this.stores.xxx1.query).then(res => {
        this.stores.xxx1.data = res.data.data;
        this.stores.xxx1.query.totalCount = res.data.totalCount;
      });
    },

    handleDelete(row) {
      //console.log(row.clientUuid);
      this.doDelete(row.clientuuid);
    },
    doDelete(ids) {
      console.log(ids);
      deleteCarDriver({
        ids: ids,
        usName: this.$store.state.user.userName
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.sleet();
          //this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    //刷新
    handleRefresh_xxx() {
      this.sleet();
    },
    //行选框的改变
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //排序改变(忽略?)
    handleSortChange(column) {
      this.stores.user.query.sort.direction = column.order;
      this.stores.user.query.sort.field = column.key;
      this.loadVoteinitiateList();
    },
    //搜索
    handleSearchDispatch_xxx() {
      this.sleet();
    },
    //表格翻页
    handlePageChanged_xxx(page) {
      this.stores.xxx1.query.currentPage = page;
      this.sleet();
    },
    //表格页大小改变
    handlePageSizeChanged_xxx(pageSize) {
      this.stores.xxx1.query.pageSize = pageSize;
      this.sleet();
    },
    //翻页
    handlePageChanged2(page) {
      this.formModel4.buslist.query.currentPage = page;
      this.doLoadData4();
    },
    //显示条数改变
    handlePageSizeChanged2(pageSize) {
      this.formModel4.buslist.query.pageSize = pageSize;
      this.doLoadData4();
    },

    //翻页
    handlePageChanged3(page) {
      this.formModel5.carlist.query.currentPage = page;
      this.doLoadData4();
    },
    //显示条数改变
    handlePageSizeChanged3(pageSize) {
      this.formModel5.carlist.query.pageSize = pageSize;
      this.doLoadData4();
    },
    //导入
    handleImport_xxx() {},
    //提示批量(操作)删除,command为提示操作类型
    handleBatchCommand_xxx(command) {
      if (!this.selectRowsId || this.selectRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content: "<p>确定要执行当前 [删除] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand_xxx(command);
        }
      });
    },
    //批量(操作)删除,command为提示操作类型
    doBatchCommand_xxx(command) {
      batchCommand_xxx({
        command: command,
        ids: this.selectRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.sleet();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },

    //编辑时打开窗口
    handleOpenFormWindow_xxx() {
      this.formModel.opened = true;
    },
    //启用编辑的状态
    handleSwitchFormModeToEdit_xxx() {
      this.formModel.mode = "edit";
    },
    //启用添加的状态
    handleSwitchFormModeToCreate_xxx() {
      this.formModel.mode = "create";
    },
    //编辑按钮
    handleEdit_xxx(row) {
      this.handleSwitchFormModeToEdit_xxx();
      this.handleOpenFormWindow_xxx();
      //查询该条数据内容
    },
    //添加按钮
    handleShowCreateWindow() {
      //console.log(this.$store.state.user.userName);
      console.log(this.usGuid);
      this.formModel.pickerkehu = [];
      this.formModel.accountmanager = [];
      //上级客户选择框内的选择项
      this.nakehu();
      this.formModel.fields.userName = this.$store.state.user.userName;
      this.formModel.fields.userGuid = this.usGuid;
      this.formModelcanyu.fields.participantright = [];
      gealluser().then(res => {
        this.formModel.Prioritylist.picker = res.data.data;
      });
      this.handleSwitchFormModeToCreate_xxx();
      this.handleResetFormDispatch123();
      this.handleOpenFormWindow_xxx();
    },

    handleResetFormDispatch123() {
      this.$refs["fromcreat"].resetFields();
      this.formModel.fields.superiorUuid="";
    },
    //表单验证
    validateForm_xxx() {
      let _valid = false;
      this.$refs["fromcreat"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    //保存按钮

    _xxx() {
      let valid = this.validateForm_xxx();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreate_xxx();
        }
        if (this.formModel.mode === "edit") {
          this.doEdit_xxx();
        }
      }
    },

    handleSubmitCreate() {
      let valid = this.validateForm_xxx();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreate_xxx();
        }
      }
    },
    //保存添加数据
    doCreate_xxx() {
      this.formModel.fields.userGuid = this.$store.state.user.userGuid;
      console.log(this.formModel.fields);
      create1(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleResetFormDispatch123();
          this.formModel.opened = false;
          this.tableData = [];
          console.log(333);
          this.sleet();
          //this.handleRefreshMenuTreeData();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //保存编辑数据
    doEdit_xxx() {
      save_xxx(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false;
          this.sleet();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //保存-分配经理权限
    editmangerpower() {
      save_manger(this.formModefenpei).then(res => {
        this.formModefenpei.opened = false;
        this.$Message.warning("成功");
        this.sleet();
      });
    },

    //客户经理选择框内的选择项
    naaccountmanager(row) {
      this.formModefenpei.rowUuid = row.clientManager;
      this.formModefenpei.clientUuid = row.clientuuid;
      this.formModefenpei.opened = true;
      geallmanager().then(res => {
        this.formModefenpei.accountmanager = res.data.data;
      });
    },
    //取消保存
    doCancel_xxx() {
      this.formModel3.opened = false;
    },
    doLoadMenuTree(selectedGuid) {
      loadMenuTree().then(res => {
        this.sources.formSource.menuTree.data = res.data.data;
      });
    },
    handleMenuTreeSelectChange(nodes) {
      var node = nodes[0];
      if (node) {
        this.formModel2.fields.superiorUuid = node.guid;
        this.formModel2.superName = node.title;
      }
    },
    handleMenuTreeSelectChange1(nodes) {
      var node = nodes[0];
      if (node) {
        this.formModel.fields.superiorUuid = node.guid;
        this.formModel.superName = node.title;
      }
    },
    //上级客户选择框内的选择项
    nakehu() {
      GetAllCustomer().then(res => {
        console.log(res);
        this.formModel.pickerkehu = res.data.data;
        this.formModel2.pickerkehu = res.data.data;
      });
    },

    handleRefreshMenuTreeData(selectedGuid) {
      this.doLoadMenuTree(selectedGuid || null);
    },
    doLoadMenuTree2(selectedGuid) {
      loadMenuTree().then(res => {
        this.sources.formSource1.menuTree.data = res.data.data;
      });
    },
    supname(row) {
      this.handleRefreshMenuTreeData2(row.clientUuid);
    },
    handleRefreshMenuTreeData2(selectedGuid) {
      this.doLoadMenuTree2(selectedGuid || null);
    },
    handleMenuTreeSelectChange2(nodes) {
      var node = nodes[0];
      if (node) {
        this.formModel.fields.superiorUuid = node.guid;
        this.formModel.superName = node.title;
      }
    },
    loadDepartmentList1() {
      //let data = this.formMode2New.fields.clientName;
      getkehu().then(res => {
        //console.log(res);
        //console.log(res.data.data);
        //this.busEditModel.Prioritylist.picker = res.data.data;
        this.stores.xxx1.sources.clientName = res.data.data;
        //console.log(this.stores.contactPerson.sources.clientName);
      });
    },
    doloadlxrList(guid) {
      loadSimpleList({ guid: guid }).then(res => {
        // //console.log(res.data.data);
        this.xglxrlist = res.data.data;
      });
    },
    testlxr(data) {},
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
    this.sleet();
    console.log(111);
    this.nakehu();
    //this.sleet();
    this.loadOtherInfo();
    this.loaselect(); //穿梭框
    this.handleRefreshMenuTreeData();
    this.loadDepartmentList1(); //获取客户下拉框
  }
};
</script>
<style>
@import url("https://cdn.jsdelivr.net/npm/vxe-table/lib/index.css");
</style>