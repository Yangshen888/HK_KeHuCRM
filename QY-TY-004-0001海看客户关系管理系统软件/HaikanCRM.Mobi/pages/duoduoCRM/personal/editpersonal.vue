<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">修改</block>
		</cu-custom>
		<form>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">联系人名称:</view>
				<input name="input" v-model="mobile.contactName" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">所属客户</view>
				<input name="input" v-model="mobile.clientName" style="text-align:right;font-size: 15px; " disabled="don"></input>
				<!-- 				<picker @change="PickerChange2" :value="Prioritylist.clientUuid" :range="Prioritylist.picker" range-key="clientName">
					<view class="picker">
						{{Prioritylist.index2>-1?Prioritylist.picker[Prioritylist.index2].clientName:'请选择客户'}}
					</view>
				</picker> -->
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
				<input name="input" v-model="mobile.email" style="text-align:right ;font-size: 15px;"></input>
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
			<!-- 			<view class="cu-form-group">
				<view class="title">手机号码:</view>
				<input name="input" v-model="mobile.bgPhone" style="text-align:right "></input>
			</view>
			<view class="cu-form-group">
				<view class="title">座机号码:</view>
				<input name="input" v-model="mobile.zjPhone" style="text-align:right "></input>
			</view> -->
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">办公地址:</view>
				<input name="input" v-model="mobile.officeAddress" style="text-align:right;font-size: 15px; "></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">家庭地址:</view>
				<input name="input" v-model="mobile.familyAddress" style="text-align:right;font-size: 15px; "></input>
			</view>
			<!-- !!!!! placeholder 在ios表现有偏移 建议使用 第一种样式 -->
			<view class="cu-form-group align-start" style="height: 120px;">
				<view class="title" style="font-size: 15px;">备注:</view>
				<textarea maxlength="-1" @input="textareaBInput" v-model="mobile.remarks" style="text-align:right;font-size: 15px; "></textarea>
			</view>
		</form>

		<view class="padding flex flex-direction">
			<button class="cu-btn bg-blue margin-tb-sm lg" style=" border-radius:20px;" @tap="geturl">确定</button>
			<button class="cu-btn bg-gray margin-tb-sm lg" style=" border-radius:20px;" @tap="quxiao">取消</button>
		</view>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		gealluser,
		AppShowContactId,
		AppEditContact,
	} from '@/api/clien/personal.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				indexTime: -1,
				//客户
				Prioritylist: {
					picker: [{
						clientUuid: "",
						clientName: "",
					}],
					index2: -1
				},
				picker1: [
					'男', '女', '未知',
				],
				don: "on",
				modalName: null,
				textareaBValue: '',
				modifyMobile: false,
				index1: -1,
				id: 0,
				contactName: "",
				clientName: "",
				title: "",
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
					contactPersonUuid: "",
					clientUuid: "",
					contactName: "",
					clientName: "",
					title: "",
					call: "",
					phone: "",
					email: "",
					dateBirth:"",
					sex: "",
					weChat: "",
					tencentQq: "",
					bgPhone: "",
					zjPhone: "",
					officeAddress: "",
					familyAddress: "",
					remarks: "",
					id: "",
				},


			};
		},
		methods: {
			TimeChange(e) {
				this.mobile.dateBirth = e.detail.value
				this.indexTime = 0;
			},
			back() {
				this.$router.go(-1);
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			PickerChange1(e) {
				this.index1 = e.detail.value
				this.mobile.sex = this.index1
				console.log(this.mobile.sex)
			},
			PickerChange2(e) {
				console.log(e);
				this.Prioritylist.index2 = e.detail.value
				this.mobile.clientUuid = this.Prioritylist.picker[this.Prioritylist.index2].clientUuid
			},
			hideModal(e) {
				this.modalName = null;
				uni.redirectTo({
					url: '/pages/duoduoCRM/clien/clien',
				});
			},
			geturl() {
				if (this.mobile.contactName == '') {
					uni.showToast({
						title: '请填写客户名称！',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				}
				var regn = /^[a-zA-Z\u4e00-\u9fa5]+$/;
				if (!regn.test(this.mobile.contactName)) {
					uni.showToast({
						icon: 'none',
						title: '客户名称不合法'
					});
					return;
				}


				//console.log(1);
				let data = {
					ContactPersonUuid: this.mobile.contactPersonUuid,
					ContactName: this.mobile.contactName,
					ClientUuid: this.mobile.clientUuid,
					Title: this.mobile.title,
					Call: this.mobile.call,
					DateBirth: this.mobile.dateBirth,
					Phone: this.mobile.phone,
					Email: this.mobile.email,
					WeChat: this.mobile.weChat,
					TencentQq: this.mobile.tencentQq,
					BgPhone: this.mobile.bgPhone,
					ZjPhone: this.mobile.zjPhone,
					OfficeAddress: this.mobile.officeAddress,
					FamilyAddress: this.mobile.familyAddress,
					Remarks: this.mobile.remarks,
					Sex: this.index1,
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.realName,
				};
				//console.log(data);
				AppEditContact(JSON.stringify(data)).then(res => {
					console.log(res);
					this.info = res.data.message;
					// this.modalName = 'show';
					if (this.info == '成功') {
						uni.showToast({
							title: '修改成功',
							icon: 'none'
						});
						uni.navigateTo({
							url: '/pages/duoduoCRM/personal/personal'
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
			quxiao() {
				uni.redirectTo({
					url: '/pages/duoduoCRM/personal/personal',
				});
			}
		},
		onLoad(opt) {
			this.id = opt.id;
			let data = {
				id: opt.id
			};
			let datas = getApp().globalData.SystemUserUuid;
			gealluser(datas).then(res => {
				this.Prioritylist.picker = res.data.data
			});
			AppShowContactId(data).then(res => {
				this.mobile = res.data.data[0];
				console.log(9898989);
				console.log(this.mobile);
				if(this.mobile.dateBirth != ''){
					this.indexTime=0;
				}
				this.index1 = (parseInt(this.mobile.sex) - 1).toString();
				console.log(this.mobile);
				console.log(this.index1);
				for (let i = 0; i < this.Prioritylist.picker.length; i++) {
					if (this.Prioritylist.picker[i].clientUuid == this.mobile.clientUuid) {
						this.Prioritylist.index2 = i;
					}
				}
			});

		}
	}
</script>

<style>
	.cu-form-group {
		height: 90rpx;
	}

	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}
</style>
