<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">编辑个人信息</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">姓名:</view>
				<input name="input" style="text-align:right" v-model="mobile.realName" disabled="disabled" />
			</view>
			<view class="cu-form-group">
				<view class="title">性别</view>
				<picker @change="changeSex" :value="indexSex" :range="pickerSex">
					<view class="picker">{{ indexSex > -1 ? pickerSex[indexSex] : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">手机:</view>
				<input name="input" style="text-align:right" v-model="mobile.phone" />
			</view>
			<view class="cu-form-group">
				<view class="title">邮箱:</view>
				<input name="input" style="text-align:right" v-model="mobile.oldCard" />
			</view>
			<view class="cu-form-group">
				<view class="title">部门</view>
				<picker @change="changeType" :value="indexType" :range="pickerType">
					<view class="picker">{{ indexType > -1 ? pickerType[indexType] : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">状态</view>
				<picker @change="changeStage" :value="indexStage" :range="pickerStage">
					<view class="picker">{{ indexStage > -1 ? pickerStage[indexStage] : '请选择' }}</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">结单日期</view>
				<input name="input" style="text-align:right" disabled="disabled" v-model="mobile.addTime" />
			</view>

			<view class="padding flex flex-direction">
				<button class="cu-btn bg-red margin-tb-sm lg" style="background-color: #0081FF;" @tap="geturl">确定</button>
				<button class="cu-btn bg-red margin-tb-sm lg" style="background-color: #d6d4ff;" @tap="quxiao">取消</button>
			</view>
		</form>
	</view>
</template>

<script>
import { showmantion, addmantion } from '@/api/information/information.js';

export default {
	data() {
		return {
			pickerType: ['测试部', '技术部', '实施部', '市场部'],
			indexType: -1,
			pickerSex: ['男', '女'],
			indexSex: -1,
			pickerStage: ['在岗', '离岗'],
			indexStage: -1,
			indexTime: -1,
			mobile: {
				systemUserUuid: '',
				phone: '',
				realName: '',
				types: '',
				sex: '',
				oldCard: '',
				addTime: '',
				zaiGang: ''
			}
		};
	},
	methods: {
		back() {
			this.$router.go(-1);
		},
		changeType(e) {
			this.indexType = e.detail.value;
			this.mobile.types = this.pickerType[this.indexType];
		},
		changeSex(e) {
			this.indexSex = e.detail.value;
			this.mobile.sex = this.pickerSex[this.indexSex];
		},
		changeStage(e) {
			this.indexStage = e.detail.value;
			this.mobile.zaiGang = this.pickerStage[this.indexStage];
		},
		geturl() {
			var reg = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
			if (!reg.test(this.mobile.phone)) {
				uni.showToast({
					icon: 'none',
					title: '手机号码不合法'
				});
				return;
			}
			var regn = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
			if (!regn.test(this.mobile.oldCard)) {
				uni.showToast({
					icon: 'none',
					title: '邮箱不合法'
				});
				return;
			}
			let data = this.mobile;
			console.log(data);
			addmantion(JSON.stringify(data)).then(res => {
				if (res.data.code == 200) {
					uni.showToast({
						title: '添加成功',
						icon: 'none'
					});
								uni.navigateBack({
									delta: 1,
									animationDuration: 200
								});
				}
			});
		},
		quxiao(){
			uni.navigateBack({
				delta: 1,
				animationDuration: 200
			});
		}
	},
	onLoad(opt) {
		let data = opt.SystemUserUuid;
		showmantion(data).then(res => {
			this.mobile = res.data.data;
			for (let i = 0; i < this.pickerType.length; i++) {
				if (this.pickerType[i] == res.data.data.types) {
					this.indexType = i;
				}
			}
			for (let i = 0; i < this.pickerSex.length; i++) {
				if (this.pickerSex[i] == res.data.data.sex) {
					this.indexSex = i;
				}
			}
		});
	}
};
</script>

<style>
.cu-form-group .title {
	min-width: calc(4em + 15px);
}
</style>
