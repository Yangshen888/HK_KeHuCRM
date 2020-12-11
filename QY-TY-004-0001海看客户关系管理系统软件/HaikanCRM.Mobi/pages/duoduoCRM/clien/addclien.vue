<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">添加客户</block>
		</cu-custom>
		<form>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">*客户</view>
				<input placeholder="请输入客户名称" name="input" v-model="mobile.clienName" style="text-align:right; font-size: 15px; "></input>
			</view>
			<!-- 			<view class="cu-form-group margin-top">
				<input placeholder="客户经理" name="ClientManager"></input>
			</view> -->
			<view class="cu-form-group" style="height: 90rpx;">
				<view class="title" style="font-size: 15px;">上级客户:</view>
				<!-- 				<picker @change="PickerChange6" :value="Prioritylist6.index6" :range="Prioritylist6.picke6" range-key="superiorName">
					<view class="picker6">
						{{ Prioritylist6.index6 > -1 ? Prioritylist6.picker6[Prioritylist6.index6].superiorName : '如果不选,默认一级客户' }}
					</view>
				</picker> -->
				<str-autocomplete :stringList="company" :importvalue="title" @select="selectOne" @change="textChange"
				 highlightColor="#FF0000" style="font-size: 15px;" v-model="title"></str-autocomplete>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">客户状态</view>
				<picker @change="PickerChange2" :value="index2" :range="picker2">
					<view class="picker">
						{{index2>-1?picker2[index2]:'请选择客户状态'}}
					</view>
				</picker>
			</view>
			<!-- 			<view class="cu-form-group">
				<view class="title">电话号码</view>
				<input placeholder="请输入电话号码" name="input" v-model="mobile.clientPhone" style="text-align:right "></input>
				<view class="cu-capsule radius">
					<view class="cu-tag line-blue">
						中国大陆
					</view>
				</view>
			</view> -->
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">类型</view>
				<picker @change="PickerChange3" :value="index3" :range="picker3">
					<view class="picker">
						{{index3>-1?picker3[index3]:'请选择类型'}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group ">
				<view class="title" style="font-size: 15px;">*行业</view>

				<picker @change="PickerChange4" :value="Prioritylist4.industryUuid" :range="Prioritylist4.picker4" range-key="industryName">
					<view class="picker">
						{{Prioritylist4.index4>-1?Prioritylist4.picker4[Prioritylist4.index4].industryName:'请选择行业'}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">所在地区</view>
				<picker @change="PickerChange1" :value="index1" :range="picker1">
					<view class="picker">
						{{index1>-1?picker1[index1]:'请选择所在地区'}}
					</view>
				</picker>
			</view>

			<!-- 			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">参与人：</view>
				<input placeholder="请选择参与人" :value="realName" name="input" disabled="on"></input>
				<view class="action">
					<button class="cu-btn bg-green shadow" @tap="showModal" data-target="ChooseModal">选择</button>
				</view>
			</view> -->
<!-- 			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">客户生日：</view>
				<picker mode="date" :value="mobile.clientBirthday" @change="TimeChange">
					<view class="picker">
						<view class="picker">{{ indexTime > -1 ? mobile.clientBirthday : '请选择' }}</view>
					</view>
				</picker>
			</view> -->

			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">邮件</view>
				<input placeholder="请输入邮件" name="input" v-model="mobile.clientEmail" style="text-align:right; font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">传真</view>
				<input placeholder="请输入传真" name="input" v-model="mobile.clientFax" style="text-align:right; font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">邮编</view>
				<input placeholder="请输入邮编" name="input" v-model="mobile.clientPostcode" style="text-align:right; font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">网站</view>
				<input placeholder="请输入网站" name="input" v-model="mobile.clientWebsite" style="text-align:right; font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">地址</view>
				<input placeholder="请输入地址" name="input" v-model="mobile.clientAddress" style="text-align:right; font-size: 15px; "></input>
			</view>
			<!-- !!!!! placeholder 在ios表现有偏移 建议使用 第一种样式 -->
			<view class="cu-form-group align-start" style="height: 120px;">
				<view class="title" style="font-size: 15px;">备注</view>
				<textarea maxlength="-1" @input="textareaBInput" placeholder="请输入备注信息" v-model="mobile.remarks" style="text-align:right; font-size: 15px; "></textarea>
			</view>



			<!-- ！！！！！参与人选择数据已放这里，放上面会影响样式 -->
			<!-- 						<view class="cu-modal bottom-modal" :class="modalName=='ChooseModal'?'show':''" @tap="hideModal11">
				<view class="cu-dialog" @tap.stop="">
					<view class="cu-bar bg-white">
						<view class="action text-blue" @tap="hideModal11">取消</view>
						<view class="action text-green" @tap="hideModal11">确定</view>
					</view>
					<view class="grid col-3 padding-sm">
						<view v-for="(item,index) in checkbox" class="padding-xs" :key="index">
							<button class="cu-btn orange lg block" :class="item.checkeds?'bg-orange':'line-orange'" @tap="ChooseCheckbox"
							 :data-value="item.value"> {{item.name}}
							</button>
						</view>
					</view>
				</view>
			</view> -->
		</form>
		<view class="padding flex flex-direction">
			<button class="cu-btn bg-blue margin-tb-sm lg" style=" border-radius:20px;" @tap="geturl">确定</button>
			<button class="cu-btn bg-gray margin-tb-sm lg" style=" border-radius:20px;" @tap="quxiao">取消</button>
		</view>

	</view>
</template>

<script>
	import strAutocomplete from '@/components/str-autocomplete/str-autocomplete.vue';
	import {
		formateDate
	} from '@/global/utils.js';
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		VagueSelectBusiness,
		VagueSelectContact,
		VagueSelectCustomer,
		AppCustomerAdd,
		GelUser,
		GetCustomerDropdown,
		GetCustomerIndustry,
	} from '@/api/clien/clien.js';
	export default {
		components: {
			strAutocomplete,
			uniIcon,
			uniPopup
		},
		data() {
			return {
				title: "",
				company: [],
				company1: [],
				company2: [],
				Prioritylist6: {
					picker6: [{
						superiorUuid: "",
						superiorName: "",
					}],
					index6: -1
				},
				systemUserUuid: "",
				realName: "",
				checkbox: [],
				modalName: null,
				modalNamefuze: null,
				textareaBValue: '',
				modifyMobile: false,
				clienName: "",
				clmoName: "",
				clientPhone: "",
				clientEmail: "",
				clientFax: "",
				clientPostcode: "",
				clientWebsite: "",
				clientAddress: "",
				remarks: "",
				info: 'xxxxx',
				indexTime: -1,
				index1: -1,
				index2: -1,
				index3: -1,
				index4: -1,
				mobile: {
					clienName: '',
					clientPhone: '',
					clientEmail: '',
					clientFax: '',
					clientPostcode: '',
					clientWebsite: '',
					clientAddress: '',
					remarks: '',
					systemUserUuid: '',
					realName: '',
					clientBirthday: '',
					superiorUuid: '',
					superiorName: '',
					clientIndustry: ','
				},
				picker1: [
					'河北省', '山西省', '辽宁省',
					'吉林省', '黑龙江省', '江苏省',
					'浙江省', '安徽省', '福建省',
					'江西省', '山东省', '河南省',
					'湖北省', '湖南省', '广东省',
					'海南省', '四川省', '贵州省',
					'云南省', '陕西省', '甘肃省',
					'青海省', '台湾省', '内蒙古自治区',
					'广西壮族自治区', '西藏自治区', '宁夏回族自治区',
					'新疆维吾尔自治区', '北京市', '天津市',
					'上海市', '重庆市', '香港特别行政区',
					'澳门特别行政区',
				],
				picker2: [
					'失败', '成功', '潜在',
					'基础', '冻结',
				],
				picker3: [
					'竞争对手', '中介机构', '代理商',
					'公关媒体', '外部资源', '供应商',
					'客户', '银行', '合作伙伴', '其他',
				],

				Prioritylist4: {
					picker4: [{
						industryUuid: "",
						industryName: "",
					}],
					index4: -1
				},
				// 'IT', '机械', '金融',
				// '销售', '畜牧业', '房地产',
				// '服务', '互联网', '其他行业',
			};
		},
		methods: {

			selectOne(opt) {
				this.mobile.clmoName = opt
				console.log(opt)
			},
			textChange(opt) {
				//console.log("textchange:" + opt)
			},

			back() {
				this.$router.go(-1);
			},
			showModal(e) {
				this.modalName = e.currentTarget.dataset.target
				this.realName = this.mobile.systemUserUuid = '';
				//单选人员绑定
				// gealluser().then(res => {
				// 	this.radiobt = res.data.data
				// })
			},
			//确定(关闭选人窗口)
			hideModal11(e) {
				// systemUserUuid:"",
				// realName:"",
				this.modalName = null
				this.modalNamefuze = null
				//多选
				for (let i = 0; i < this.checkbox.length; i++) {
					if (this.checkbox[i].checkeds == true) {
						this.realName += this.checkbox[i].name + ",";
						this.mobile.systemUserUuid += this.checkbox[i].value + ",";
					}
				}
			},
			//选择参与人员
			ChooseCheckbox(e) {
				let items = this.checkbox;
				let values = e.currentTarget.dataset.value;
				for (let i = 0, lenI = items.length; i < lenI; ++i) {
					if (items[i].value == values) {
						items[i].checkeds = !items[i].checkeds;
						break
					}
				}
			},

			TimeChange(e) {
				this.mobile.clientBirthday = e.detail.value
				this.indexTime = 0;
				console.log(this.mobile.clientBirthday);
			},
			PickerChange1(e) {
				this.index1 = e.detail.value
			},
			PickerChange2(e) {
				this.index2 = e.detail.value
			},
			PickerChange3(e) {
				this.index3 = e.detail.value
			},
			PickerChange4(e) {
				//this.index4 = e.detail.value
				this.Prioritylist4.index4 = e.target.value
				this.mobile.clientIndustry = this.Prioritylist4.picker4[this.Prioritylist4.index4].industryUuid
				console.log(this.mobile.clientIndustry);
			},
			PickerChange6(e) {
				console.log(e);
				this.Prioritylist6.index6 = e.detail.value
				this.mobile.superiorUuid = this.Prioritylist6.picker6[this.Prioritylist6.index6].superiorUuid
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value

			},
			geturl() {
				if (this.mobile.clienName == '') {
					uni.showToast({
						title: '请填写客户',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				};
				if (this.mobile.clientIndustry == ',' ) {
					uni.showToast({
						title: '请选择行业',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				};
				
				var regn = /^[a-zA-Z\u4e00-\u9fa5]+$/;
				if (!regn.test(this.mobile.clienName)) {
					uni.showToast({
						icon: 'none',
						title: '客户名称不合法'
					});
					return;
				}
				// var reg = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
				// if (!reg.test(this.mobile.clientPhone)) {
				// 	uni.showToast({
				// 		icon: 'none',
				// 		title: '手机号码不合法'
				// 	});
				// 	return;
				// }
				//console.log(this.mobile.systemUserUuid);
				let data = {
					ClienName: this.mobile.clienName,
					ClmoName: this.mobile.clmoName,
					// ClientPhone: this.mobile.clientPhone,
					ClientEmail: this.mobile.clientEmail,
					ClientFax: this.mobile.clientFax,
					ClientPostcode: this.mobile.clientPostcode,
					ClientWebsite: this.mobile.clientWebsite,
					ClientAddress: this.mobile.clientAddress,
					Remarks: this.mobile.remarks,
					ClientArea: this.picker1[this.index1],
					ClientStatus: this.picker2[this.index2],
					ClientType: this.picker3[this.index3],
					ClientIndustry: this.mobile.clientIndustry,
					SystemUserUuid: this.mobile.systemUserUuid,
					//SuperiorUuid:this.mobile.superiorUuid,
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.realName,
					ClientManager: getApp().globalData.SystemUserUuid,
					ClientBirthday: this.mobile.clientBirthday,
				};
				//console.log(this.mobile.clientBirthday);
				//JSON.stringify(data)
				AppCustomerAdd(JSON.stringify(data)).then(res => {
					console.log(res);
					this.info = res.data.message;
					// this.modalName = 'show';
					if (this.info == '成功') {
						uni.showToast({
							title: '添加成功',
							icon: 'none'
						});
						uni.navigateTo({
							url: '/pages/duoduoCRM/clien/clien'
						});
					} else {
						uni.showToast({
							title: '添加失败,请检查所输入信息是否合法',
							icon: 'none'
						});
						return;
					}
				});
			},
			quxiao() {
				uni.redirectTo({
					url: '/pages/duoduoCRM/clien/clien',
				});
			},
			hideModal(e) {
				this.modalName = null;
				uni.redirectTo({
					url: '/pages/duoduoCRM/clien/clien',
				});
			},
		},
		onLoad() {
			GelUser().then(res => {
				this.checkbox = res.data.data
				//console.log(this.checkbox);
			});
			GetCustomerDropdown().then(res => {
				this.Prioritylist6.picker6 = res.data.data
			});

			GetCustomerIndustry().then(res => {
				this.Prioritylist4.picker4 = res.data.data
				console.log(this.picker4);
			});
			VagueSelectCustomer().then(res => {
				this.company = res.data.data;
				for (let i = 0; i < res.data.data.length; i++) {
					this.company[i] = res.data.data[i].clientName
				}
				//console.log(this.company);
			});
		},
	}
</script>

<style>
	.cu-form-group {
		height: 90rpx;
	}

	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}

	.cu-form-group uni-picker::after {
		line-height: 80rpx;
	}
</style>
