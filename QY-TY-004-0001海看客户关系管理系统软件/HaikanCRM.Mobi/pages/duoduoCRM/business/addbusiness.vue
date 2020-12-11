<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">{{ mobile.businessUuid == '' ? '添加商机' : '修改商机' }}</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title shuai" style="font-size: 15px;">*商机名称:</view>
				<input name="input" style="text-align:right; font-size: 15px;" v-model="mobile.businessName" />
			</view>
<!-- 			<view class="cu-form-group margin-top" v-if="clienid2">
				<view class="title shuai" style="font-size: 15px;">*所属客户</view>
				<input name="input" style="text-align:right; font-size: 15px;" v-model="mobile.clientName"  disabled="don"/>
			</view> -->
			<view class="cu-form-group" v-show="clienid==1">
				<view class="title" style="font-size: 15px;">*所属客户</view>
				<!-- 				<picker @change="PickerChange2" :value="Prioritylist.index2" :range="Prioritylist.picker" range-key="clientName">
					<view class="picker">{{ Prioritylist.index2 > -1 ? Prioritylist.picker[Prioritylist.index2].clientName : '请选择' }}</view>
				</picker> -->
				<str-autocomplete :stringList="company" :importvalue="title" @select="selectOne" @change="textChange"
				 highlightColor="#FF0000" v-model="mobile.clienName"></str-autocomplete>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">商机来源</view>
				<picker @change="changeSource" :value="indexSource" :range="pickerSource">
					<view class="picker">{{ indexSource > -1 ? pickerSource[indexSource] : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">商机类型</view>
				<picker @change="changeType" :value="indexType" :range="pickerType">
					<view class="picker">{{ indexType > -1 ? pickerType[indexType] : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">项目预算:</view><!-- type="number" -->
				<input name="input" style="text-align:right; font-size: 15px; " v-model="mobile.salesAmount" />
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">商机赢率</view>
				<picker @change="changeWinrate" :value="indexWinrate" :range="pickerWinrate">
					<view class="picker">{{ indexWinrate > -1 ? pickerWinrate[indexWinrate] : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">商机阶段</view>
				<picker @change="PickerChange1" :value="index1" :range="pickerStage">
					<view class="picker">{{ index1 > -1 ? pickerStage[index1] : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">相关联系人：</view>
				<input placeholder="相关联系人" :value="realName" name="input" disabled="on"></input>
				<view class="action">
					<button class="cu-btn bg-green shadow" @tap="showModal" data-target="ChooseModal">选择</button>
				</view>
			</view>



			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">预计成交日期</view>
				<picker mode="date" :value="mobile.checkTime" @change="changeTime">
					<view class="picker">{{ indexTime > -1 ? mobile.checkTime : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group align-start" style="height: 120px;">
				<view class="title" style="font-size: 15px;">备注:</view>
				<textarea maxlength="-1" v-model="mobile.remarks" style="font-size: 15px;"></textarea>
			</view>

			<!-- ！！！！！参与人选择数据已放这里，放上面会影响样式 -->
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
			</view>


			<view class="padding flex flex-direction">
				<button class="cu-btn bg-red margin-tb-sm lg" style="background-color: #0081FF; border-radius:20px;" @tap="geturl">确定</button>
				<button class="cu-btn bg-red margin-tb-sm lg" style="background-color: #d6d4ff; border-radius:20px;" @tap="quxiao">取消</button>
			</view>
		</form>
	</view>
</template>

<script>
	//import http from '@/components/utils/http.js'
	import strAutocomplete from '@/components/str-autocomplete/str-autocomplete.vue';
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		formateDate
	} from '@/global/utils.js';
	import {
		VagueSelectBusiness,
		VagueSelectContact,
		VagueSelectCustomer,
		GelUser,
		AppbusRelatedConName,
		AppbusRelatedConGuid,
	} from '@/api/clien/clien.js';
	import {
		BusAdd,
		GetBusSelectCus,
		BusView
	} from '@/api/business/business.js';
	export default {
		components: {
			strAutocomplete,
			uniIcon,
			uniPopup
		},
		data() {
			return {
				don:'on',
				ClientUuid: '',
				title: '',
				xiangguanlxr: 1,
				clienid: 1,
				clienid2: false,
				company: [],
				company1: [],
				company2: [],
				systemUserUuid: "",
				realName: "",
				checkbox: [],
				modalName: null,
				modalNamefuze: null,
				textareaBValue: '',
				modifyMobile: false,
				//客户
				Prioritylist: {
					picker: [{
						clientUuid: '',
						clientName: ''
					}],
					index2: -1
				},
				index1: -1,
				pickerSource: ['独立开发', '客户介绍', '其它广告', '来电咨询'],
				indexSource: -1,
				pickerType: ['普通机会', '重要机会', '特殊机会'],
				indexType: -1,
				pickerStage: ['沟通调研', '方案策划', '招投标', '合同签订', '已关闭'],
				pickerWinrate: [],
				indexWinrate: -1,
				indexTime: -1,
				mobile: {
					clientName:'',
					clienName: '',
					clientUuid: '',
					businessStage: '',
					salesAmount: '',
					winrate: '',
					businessSource: '',
					businessType: '',
					checkTime: '',
					remarks: '',
					contactBusName: '',
					businessName: '',
					businessUuid: '',
					systemUserUuid: '',
					systemUserName: '',
					userGuid: '',
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.realName,
				}
			};
		},
		methods: {

			async sjlxr(e) {
				console.log(76766);
				this.checkbox = [];
				//this.mobile.clienName = "hp";
				let data = {
					guid: e,
				};
				await AppbusRelatedConName(JSON.stringify(data)).then(res => {
					console.log(2233);
					console.log(res);
					this.checkbox = res.data.data;
				});
			},
			async selectOne(op) {
				this.mobile.clienName = op;
				console.log(1133);
				if (this.mobile.clienName != '') {
					await this.sjlxr(this.mobile.clienName);
				}
				console.log(this.mobile.clienName);
			},
			textChange(op) {
				//console.log("textchange:" + opt)
			},
			back() {
				this.$router.go(-1);
			},
			showModal(e) {
				this.modalName = e.currentTarget.dataset.target
				this.realName = this.mobile.systemUserUuid = '';
				//单选人员绑定
				// GetBusSelectCus().then(res => {
				// 	this.radiobt = res.data.data
				// })
			},
			//确定(关闭选人窗口)
			hideModal11(e) {
				// systemUserUuid:"",
				// realName:"",
				this.modalName = null
				this.modalNamefuze = null
				console.log(7766)
				console.log(this.checkbox)
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
			changeType(e) {
				this.indexType = e.detail.value;
				this.mobile.businessType = this.pickerType[this.indexType];
			},
			changeSource(e) {
				this.indexSource = e.detail.value;
				this.mobile.businessSource = this.pickerSource[this.indexSource];
			},
			PickerChange1(e) {
				this.index1 = e.detail.value;
				this.mobile.businessStage = this.pickerStage[this.index1];
			},
			PickerChange2(e) {
				this.Prioritylist.index2 = e.target.value;
				this.mobile.clientUuid = this.Prioritylist.picker[this.Prioritylist.index2].clientUuid;
			},
			changeWinrate(e) {
				this.indexWinrate = e.detail.value;
				this.mobile.winrate = this.pickerWinrate[this.indexWinrate];
			},
			changeTime(e) {
				this.mobile.checkTime = e.detail.value;
				this.indexTime = 0;
			},
			geturl() {
				if (this.mobile.businessName == '') {
					uni.showToast({
						title: '请填写商机名称！',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				}
				let regn = /^[a-zA-Z\u4e00-\u9fa5]+$/;
				if (!regn.test(this.mobile.businessName)) {
					uni.showToast({
						icon: 'none',
						title: '商机名称不合法'
					});
					return;
				}

				if (this.mobile.salesAmount != '') {
					let regn2 = /^\d+$|^\d+[.]?\d+$/;
					if (!regn2.test(this.mobile.salesAmount)) {
						uni.showToast({
							icon: 'none',
							title: '金额输入不合法'
						});
						return;
					}
				}

				if (this.mobile.clienName == '') {
					uni.showToast({
						title: '请选择所属客户！',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				}
				if (this.indexTime == -1) {
					this.mobile.checkTime = '';
				}
				this.mobile.userGuid = getApp().globalData.SystemUserUuid;
				this.mobile.usName = getApp().globalData.realName,
					console.log(this.mobile);
				let data = {
					clienName: this.mobile.clienName,
					clientUuid: this.mobile.clientUuid,
					businessStage: this.mobile.businessStage,
					salesAmount: this.mobile.salesAmount,
					winrate: this.mobile.winrate,
					winrate: this.mobile.winrate,
					businessSource: this.mobile.businessSource,
					businessType: this.mobile.businessType,
					checkTime: this.mobile.checkTime,
					remarks: this.mobile.remarks,
					contactBusName: this.mobile.contactBusName != null ? this.mobile.contactBusName.split(",") : [],
					businessName: this.mobile.businessName,
					businessUuid: this.mobile.businessUuid,
					systemUserUuid: this.mobile.systemUserUuid,
					systemUserName: this.mobile.systemUserName,
					userGuid: this.mobile.userGuid,
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.realName,
				}
				//JSON.stringify(data)
				console.log(898988);
				console.log(data);
				BusAdd(JSON.stringify(data)).then(res => {
					console.log(112244);
					console.log(res);
					if (res.data.code == 200) {
						if (this.mobile.businessUuid != '') {
							uni.showToast({
								title: '修改商机成功',
								icon: 'none'
							});
							uni.redirectTo({
								url: '/pages/duoduoCRM/business/business?ClientUuid=' + this.ClientUuid,
							})
						} else {
							uni.showToast({
								title: '添加商机成功',
								icon: 'none'
							});
							uni.redirectTo({
								url: '/pages/duoduoCRM/business/business?ClientUuid=' + this.ClientUuid,
							})
						}
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
				uni.navigateBack({
					delta: 1,
					animationDuration: 200
				});
			}
		},
		onLoad(opt) {
			if (this.mobile.clienName != '') {
				AppbusRelatedConName({
					guid: this.mobile.clienName
				}).then(res => {
					console.log(433)
					console.log(res)
					this.checkbox = res.data.data;
				});
			};

			VagueSelectCustomer().then(res => {
				this.company = res.data.data;
				for (let i = 0; i < res.data.data.length; i++) {
					this.company[i] = res.data.data[i].clientName
				}
				console.log(666444);
				console.log(this.company);
			});
			//getApp().globalData.SystemUserUuid
			let datas = getApp().globalData.SystemUserUuid;
			// GelUser().then(res => {
			// 	this.checkbox = res.data.data
			// 	console.log(this.checkbox);
			// });
			GetBusSelectCus(datas).then(res => {
				this.Prioritylist.picker = res.data.data;
			});
			for (let i = 0; i < 101; i++) {
				this.pickerWinrate[i] = i + '%';
			}
			this.ClientUuid = opt.ClientUuid;
			let data = opt.businessUuid;
			if (data != undefined) {
				//this.clienid = 0;
				//this.clienid2 = true;
				this.xiangguanlxr = 0;
				this.mobile.businessUuid = data;
				BusView(data).then(res => {
					if (res.data.code == 200) {
						if (res.data.data[0].clientUuid != "") {
							AppbusRelatedConGuid(
								JSON.stringify({
									guid: res.data.data[0].clientUuid
								})).then(res2 => {
								let nlist = res2.data.data;
								if(res.data.data[0].contactBusName != null){
									let dlist = res.data.data[0].contactBusName.split(",");
									for (let i = 0; i < dlist.length; i++) {
										let index = nlist.findIndex(x => x.value == dlist[i]);
										console.log(index);
										if (index >= 0) {
											nlist[index].checkeds = true;
											this.realName += nlist[index].name + ",";
										}
									}
								}
								this.checkbox = nlist;
							});
						}
						this.mobile.systemUserUuid = res.data.data[0].systemUserUuid;
						this.mobile.systemUserName = res.data.data[0].systemUserName;
						this.mobile = res.data.data[0];
						this.mobile.clienName = res.data.data[0].clientName;
						console.log(11);
						console.log(this.mobile);
						for (let i = 0; i < this.pickerSource.length; i++) {
							if (this.pickerSource[i] == res.data.data[0].businessSource) {
								this.indexSource = i;
							}
						}
						for (let i = 0; i < this.pickerType.length; i++) {
							if (this.pickerType[i] == res.data.data[0].businessType) {
								this.indexType = i;
							}
						}
						for (let i = 0; i < this.pickerStage.length; i++) {
							if (this.pickerStage[i] == res.data.data[0].businessStage) {
								this.index1 = i;
							}
						}
						for (let i = 0; i < this.pickerWinrate.length; i++) {
							if (this.pickerWinrate[i] == res.data.data[0].winrate) {
								this.indexWinrate = i;
							}
						}
						if (res.data.data[0].checkTime != '') {
							this.indexTime = 0;
							this.mobile.checkTime = res.data.data[0].checkTime;
						}
					}
				});
			}
		}
	};
</script>

<style>
	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}

	.shuai {}
</style>
