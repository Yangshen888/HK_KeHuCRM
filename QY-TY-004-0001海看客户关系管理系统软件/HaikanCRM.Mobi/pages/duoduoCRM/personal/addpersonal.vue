<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">添加联系人</block>
		</cu-custom>
		<form>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">*联系人名称:</view>
				<input name="input" v-model="mobile.contactName" style="text-align:right "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">*所属客户</view>
				<!-- 				<picker @change="PickerChange2" :value="Prioritylist.clientUuid" :range="Prioritylist.picker" range-key="clientName">
					<view class="picker">
						{{Prioritylist.index2>-1?Prioritylist.picker[Prioritylist.index2].clientName:'请选择客户'}}
					</view>
				</picker> -->

				<str-autocomplete :stringList="company" :importvalue="title" @select="selectOne" @change="textChange"
				 highlightColor="#FF0000" style="font-size: 15px;" v-model="title"></str-autocomplete>

			</view>

			<!-- 			<view class="cu-form-group">
				<view class="title">头衔:</view>
				<input name="input" v-model="mobile.title" style="text-align:right "></input>
			</view> -->
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">职务:</view>
				<input name="input" v-model="mobile.call" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">电话(多个空格隔开):</view>
				<input name="input" v-model="mobile.phone" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">邮件:</view>
				<input name="input" v-model="mobile.email" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">性别</view>
				<picker @change="PickerChange1" :value="index1" :range="picker1">
					<view class="picker">
						{{index1>-1?picker1[index1]:'请选择性别'}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">生日：</view>
				<picker mode="date" :value="mobile.dateBirth" @change="TimeChange">
					<view class="picker">
						<view class="picker">{{ indexTime > -1 ? mobile.dateBirth : '请选择' }}</view>
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">微信:</view>
				<input name="input" v-model="mobile.weChat" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">QQ:</view>
				<input name="input" v-model="mobile.tencentQq" style="text-align:right;font-size: 15px; "></input>
			</view>

			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">办公地址:</view>
				<input name="input" v-model="mobile.officeAddress" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">家庭地址:</view>
				<input name="input" v-model="mobile.familyAddress" style="text-align:right;font-size: 15px; "></input>
			</view>
			<!-- !!!!! placeholder 在ios表现有偏移 建议使用 第一种样式 -->
			<view class="cu-form-group align-start">
				<view class="title" style="font-size: 15px;">备注:</view>
				<textarea maxlength="-1" @input="textareaBInput" v-model="mobile.remarks" style="text-align:right;font-size: 15px; "></textarea>
			</view>
		</form>

		<view class="padding flex flex-direction">
			<button class="cu-btn bg-blue margin-tb-sm lg" style="border-radius:20px;" @tap="geturl">确定</button>
			<button class="cu-btn bg-gray margin-tb-sm lg" style="border-radius:20px;" @tap="quxiao">取消</button>
		</view>

	</view>
</template>

<script>
	import strAutocomplete from '@/components/str-autocomplete/str-autocomplete.vue';
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		AppAddContact,
		gealluser,
	} from '@/api/clien/personal.js';
	import {
		VagueSelectBusiness,
		VagueSelectContact,
		VagueSelectCustomer,
		GetPogodaList,
	} from '@/api/clien/clien.js';
	export default {
		components: {
			strAutocomplete,
			uniIcon,
			uniPopup
		},
		data() {
			return {
				company: [],
				company1: [],
				company2: [],
				indexTime: -1,
				//客户
				Prioritylist: {
					picker: [{
						clientUuid: "",
						clientName: "",
					}],
					index2: -1
				},
				modalName: null,
				textareaBValue: '',
				modifyMobile: false,
				index1: -1,
				id: 0,
				contactName: "",
				clientName: "",
				fname: "",
				title: '',
				call: "",
				phone: "",
				email: "",
				sex: "",
				weChat: "",
				tencentQQ: "",
				bgPhone: "",
				zjPhone: "",
				officeAddress: "",
				familyAddress: "",
				remarks: "",
				mobile: {
					clientUuid: "",
					contactName: "",
					clientName: "",
					title: "",
					call: "",
					phone: "",
					email: "",
					sex: "",
					weChat: "",
					tencentQq: "",
					dateBirth:"",
					bgPhone: "",
					zjPhone: "",
					officeAddress: "",
					familyAddress: "",
					remarks: "",
					id: "",
				},
				picker1: [
					'男', '女', '未知'
				],
			};
		},


		methods: {
			TimeChange(e) {
				this.mobile.dateBirth = e.detail.value
				this.indexTime = 0;
			},
			selectOne(opt) {
				this.mobile.clientName = opt
				console.log(this.mobile.clientName)
			},
			textChange(opt) {
				//console.log("textchange:" + opt)
			},

			fuzzy_search() {
				this.stores.pogoda.systemUserUuid = getApp().globalData.SystemUserUuid;
				GetPogodaList(JSON.stringify(this.stores.pogoda)).then(res => {
					console.log(res.data.data);
					if (res.data.data == '') {
						uni.showToast({
							title: '未匹配到相关客户信息',
							icon: 'none'
						});
						return;
					} else {
						this.kw = res.data.data;
						console.log(this.kw.length);
					}
				});
			},

			back() {
				this.$router.go(-1);
			},
			PickerChange1(e) {
				this.index1 = e.detail.value
				this.mobile.sex = this.index1
				//console.log(this.mobile.sex)
			},
			PickerChange2(e) {
				this.Prioritylist.index2 = e.target.value
				this.mobile.clientUuid = this.Prioritylist.picker[this.Prioritylist.index2].clientUuid
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			geturl() {
				//console.log(1);
				if (this.mobile.contactName == '') {
					uni.showToast({
						title: '请输入联系人名称！',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				}
				// var regn =  /^[^\s]+$/;
				var regn = /^[a-zA-Z\u4e00-\u9fa5]+$/;
				if (!regn.test(this.mobile.contactName)) {
					uni.showToast({
						icon: 'none',
						title: '联系人名称不合法'
					});
					return;
				}
				if (this.mobile.clientName == '') {
					uni.showToast({
						title: '请选择客户！',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				}
				let data = {
					ContactName: this.mobile.contactName,
					ClientUuid: this.mobile.clientUuid,
					ClientName: this.mobile.clientName,
					Title: this.mobile.title,
					Call: this.mobile.call,
					Phone: this.mobile.phone,
					Email: this.mobile.email,
					WeChat: this.mobile.weChat,
					TencentQq: this.mobile.tencentQq,
					DateBirth: this.mobile.dateBirth,
					BgPhone: this.mobile.bgPhone,
					ZjPhone: this.mobile.zjPhone,
					OfficeAddress: this.mobile.officeAddress,
					FamilyAddress: this.mobile.familyAddress,
					Remarks: this.mobile.remarks,
					Sex: this.mobile.sex,
					SystemUserUuid: getApp().globalData.SystemUserUuid,
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.realName,
				};
				//console.log(data);
				AppAddContact(JSON.stringify(data)).then(res => {
					console.log(res);
					this.info = res.data.message;
					// this.modalName = 'show';
					if (this.info == '成功') {
						uni.showToast({
							title: '添加成功',
							icon: 'none'
						});
						uni.navigateTo({
							url: '/pages/duoduoCRM/personal/personal'
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
			hideModal(e) {
				this.modalName = null;
				uni.redirectTo({
					url: '/pages/duoduoCRM/personal/personal',
				});
			},
			quxiao() {
				uni.redirectTo({
					url: '/pages/duoduoCRM/personal/personal',
				});
			},



		},
		onLoad() {
			let data = getApp().globalData.SystemUserUuid;
			gealluser(data).then(res => {
					this.Prioritylist.picker = res.data.data
				}),
				VagueSelectCustomer().then(res => {
					this.company = res.data.data;
					for (let i = 0; i < res.data.data.length; i++) {
						this.company[i] = res.data.data[i].clientName
					}
					console.log(this.company);
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
