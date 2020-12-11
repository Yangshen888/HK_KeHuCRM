<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">修改</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title" style="font-size: 15px;">客户</view>
				<input placeholder="请输入客户名称" name="input" v-model="mobile.clientName" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title" style="font-size: 15px;">客户经理</view>
				<input placeholder="客户经理" name="ClientManager" v-model="mobile.clientManager" disabled="on" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title" style="font-size: 15px;">客户状态</view>
				<picker @change="PickerChange2" :value="Prioritylist2.customerStatusUuid" :range="Prioritylist2.picker2" range-key="statusName">
					<view class="picker">
						{{Prioritylist2.index2>-1?Prioritylist2.picker2[Prioritylist2.index2].statusName:'请选择客户状态'}}
					</view>
				</picker>
			</view>

			<view class="cu-form-group margin-top">
				<view class="title" style="font-size: 15px;">类型</view>
				<picker @change="PickerChange3" :value="Prioritylist3.typeUuid" :range="Prioritylist3.picker3" range-key="typeName">
					<view class="picker">
						{{Prioritylist3.index3>-1?Prioritylist3.picker3[Prioritylist3.index3].typeName:'请选择类型'}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group margin-top">
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


			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">邮件</view>
				<input placeholder="请输入邮件" name="input" v-model="mobile.clientEmail" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">传真</view>
				<input placeholder="请输入传真" name="input" v-model="mobile.clientFax" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">邮编</view>
				<input placeholder="请输入邮编" name="input" v-model="mobile.clientPostcode" style="text-align:right;font-size: 15px; "></input>
			</view>
			<!-- 			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">参与人：</view>
				<input placeholder="请选择参与人" :value="realName" name="input" disabled="on"></input>
				<view class="action">
					<button class="cu-btn bg-green shadow" @tap="showModal" data-target="ChooseModal">选择</button>
				</view>
			</view>
			<view class="cu-modal bottom-modal" :class="modalName=='ChooseModal'?'show':''" @tap="hideModal11">
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

<!-- 			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">客户生日：</view>
				<picker mode="date" :value="mobile.clientBirthday" @change="TimeChange">
					<view class="picker">
						<view class="picker">{{ indexTime > -1 ? mobile.clientBirthday : '请选择' }}</view>
					</view>
				</picker>
			</view> -->

			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">网站</view>
				<input placeholder="请输入网站" name="input" v-model="mobile.clientWebsite" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">地址</view>
				<input placeholder="请输入地址" name="input" v-model="mobile.clientAddress" style="text-align:right;font-size: 15px; "></input>
			</view>
			<!-- !!!!! placeholder 在ios表现有偏移 建议使用 第一种样式 -->
			<view class="cu-form-group align-start">
				<view class="title" style="font-size: 15px;">备注</view>
				<textarea maxlength="-1" @input="textareaBInput" placeholder="请输入备注信息" v-model="mobile.remarks" style="text-align:right;font-size: 15px; "></textarea>
			</view>
		</form>
		<view class="padding flex flex-direction">
			<button class="cu-btn bg-red margin-tb-sm lg" style="background-color: #0081FF; border-radius:20px;" @tap="getClii">保存</button>
		</view>

	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		formateDate
	} from '@/global/utils.js';
	import {
		SavaEditCustomerInfo,
		GelUser,
		AllStatus,
		AllTypes,
		AllIndustry,
		AppGetCustomerInfo,
	} from '@/api/clien/clien.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				indexTime: -1,
				systemUserUuid: "",
				realName: "",
				checkbox: [],
				modalName: null,
				modalNamefuze: null,
				textareaBValue: '',
				modifyMobile: false,
				clientManager: "",
				clientName: "",
				clientPhone: "",
				clientEmail: "",
				clientFax: "",
				clientPostcode: "",
				clientWebsite: "",
				clientAddress: "",
				remarks: "",
				clientStatus: "",
				clientType: "",
				clientIndustry: "",
				clientArea: "",
				customerStatusUuid: "",
				industryUuid: "",
				industryName: "",
				statusName: "",
				typeUuid: "",
				typeName: "",
				info: 'xxxxx',
				index1: -1,
				mobile: {
					clientUuid: '',
					clientManager: '',
					clientName: '',
					clientPhone: '',
					clientEmail: '',
					clientFax: '',
					clientPostcode: '',
					clientWebsite: '',
					clientAddress: '',
					remarks: '',
					systemUserUuid: '',
					realName: '',
					clientStatus: '',
					clientType: '',
					clientIndustry: '',
					clientArea: '',
					customerStatusUuid: '',
					industryUuid: '',
					industryName: '',
					statusName: '',
					typeUuid: '',
					typeName: '',
					clientBirthday: '',
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
				Prioritylist2: {
					picker2: [{
						customerStatusUuid: "",
						statusName: "",
					}],
					index2: -1
				},
				Prioritylist3: {
					picker3: [{
						typeUuid: "",
						typeName: "",
					}],
					index3: -1
				},
				Prioritylist4: {
					picker4: [{
						industryUuid: "",
						industryName: "",
					}],
					index4: -1
				},

			}
		},
		methods: {
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

				console.log(values);
			},
			TimeChange(e) {
				this.mobile.clientBirthday = e.detail.value
				this.indexTime = 0;
			},
			PickerChange1(e) {
				this.index1 = e.detail.value

				console.log(e)
				console.log(this.picker1[this.index1])
				//this.mobile.clientArea = this.index1
			},
			PickerChange2(e) {
				this.Prioritylist2.index2 = e.detail.value
				this.mobile.customerStatusUuid = this.Prioritylist2.picker2[this.Prioritylist2.index2].customerStatusUuid
			},
			PickerChange3(e) {
				this.Prioritylist3.index3 = e.detail.value
				this.mobile.typeUuid = this.Prioritylist3.picker3[this.Prioritylist3.index3].typeUuid
			},
			PickerChange4(e) {
				this.Prioritylist4.index4 = e.detail.value
				this.mobile.industryUuid = this.Prioritylist4.picker4[this.Prioritylist4.index4].industryUuid
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			getClii() {
				if (this.mobile.clientName == '') {
					uni.showToast({
						title: '请填写客户名称！',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				}
				var regn = /^[a-zA-Z\u4e00-\u9fa5]+$/;
				if (!regn.test(this.mobile.clientName)) {
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
				//console.log(this.mobile);
				console.log(1212);
				let data = {
					ClientUuid: this.mobile.clientUuid,
					ClienName: this.mobile.clientName,
					// ClientPhone: this.mobile.clientPhone,
					ClientEmail: this.mobile.clientEmail,
					ClientFax: this.mobile.clientFax,
					ClientPostcode: this.mobile.clientPostcode,
					ClientWebsite: this.mobile.clientWebsite,
					ClientAddress: this.mobile.clientAddress,
					Remarks: this.mobile.remarks,
					ClientArea: this.picker1[this.index1],
					ClientStatus: this.mobile.customerStatusUuid,
					ClientType: this.mobile.typeUuid,
					ClientIndustry: this.mobile.industryUuid,
					//SystemUserUuid: this.mobile.systemUserUuid,
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.realName,
					ClientBirthday: this.mobile.clientBirthday,
				};
				console.log(data);
				SavaEditCustomerInfo(JSON.stringify(data)).then(res => {
					console.log(122);
					this.info = res.data.message;
					// this.modalName = 'show';
					if (this.info == '成功') {
						uni.showToast({
							title: '修改客户',
							icon: 'none'
						});
						uni.redirectTo({
							url: '/pages/duoduoCRM/clien/clien',
						});
					} else {
						uni.showToast({
							title: '修改失败,请检查所输入信息是否合法',
							icon: 'none'
						});
						return;
					}
				});
			},
			hideModal(e) {
				this.modalName = null;
				uni.redirectTo({
					url: '/pages/duoduoCRM/clien/clien',
				});
			},
		},
		onLoad(opt) {
			console.log(2222);
			console.log(this);
			let that=this;
			that.id = opt.clientUuid;
			let data = {
				id: opt.clientUuid
			};
			// GelUser().then(res => {
			// 	this.checkbox = res.data.data
			// 	//console.log(this.checkbox)
			// });
			AllStatus().then(res => {
				console.log(33333);
				console.log(res);
				console.log(that);
				console.log(that.Prioritylist2);
				that.Prioritylist2.picker2 = res.data.data
				
				//console.log(this.Prioritylist2.picker2)
			});
			AllTypes().then(res => {
				that.Prioritylist3.picker3 = res.data.data
				//console.log(this.Prioritylist3.picker3)				
			});
			AllIndustry().then(res => {
				that.Prioritylist4.picker4 = res.data.data
				//console.log(this.Prioritylist4.picker4)				
			});
			AppGetCustomerInfo(data).then(res => {
				console.log(res.data.data[0]);
				that.mobile = res.data.data[0];
				that.picker1[that.index1] = that.mobile.clientArea;
				that.mobile.systemUserUuid = res.data.data[0].systemUserUuid;
				that.realName = res.data.data[0].realName;
				//console.log(this.checkbox)
				if (res.data.data[0].clientBirthday != null) {
					that.indexTime = 0;
					that.mobile.clientBirthday = res.data.data[0].clientBirthday;
				}
				for (let i = 0; i < that.picker1.length; i++) {
					if (that.picker1[i] == that.mobile.clientArea) {
						that.index1 = i
					}
				}
				//console.log(this.mobile);
				for (let i = 0; i < that.Prioritylist2.picker2.length; i++) {
					if (that.Prioritylist2.picker2[i].customerStatusUuid == that.mobile.customerStatusUuid) {
						that.Prioritylist2.index2 = i;
					}
				};
				for (let o = 0; o < that.Prioritylist3.picker3.length; o++) {
					if (that.Prioritylist3.picker3[o].typeUuid == that.mobile.typeUuid) {
						that.Prioritylist3.index3 = o;
					}
				};
				for (let u = 0; u < that.Prioritylist4.picker4.length; u++) {
					if (that.Prioritylist4.picker4[u].industryUuid == that.mobile.industryUuid) {
						that.Prioritylist4.index4 = u;
					}
				};
				//console.log(this.mobile);				
			})
		},
	}
</script>

<style>
	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}
</style>
