<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">商机详情</block>
		</cu-custom>
		<form>
		<view class="cu-form-group margin-top">
			<view class="title" style="font-size: 15px;">商机名称:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.businessName" />
		</view>
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">所属客户:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.clientName" />
		</view>
<!-- 		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">客户经理:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.systemUserName" />
		</view> -->
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">商机来源:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.businessSource" />
		</view>
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">商机类型:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.businessType" />
		</view>
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">项目预算:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.salesAmount" />
		</view>
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">商机赢率:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.winrate" />
		</view>
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">商机阶段:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.businessStage" />
		</view>
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">预计成交日期:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.checkTime" />
		</view>
		<view class="cu-form-group">
			<view class="title" style="font-size: 15px;">备注:</view>
			<input name="input" style="text-align:right; font-size: 15px;" disabled="disabled" v-model="query.b.remarks" />
		</view>
			<view class="padding flex flex-direction"><button class="cu-btn bg-red margin-tb-sm lg" style="background-color: #d6d4ff; border-radius:20px;" @tap="quxiao">取消</button></view>
		</form>
	</view>
</template>

<script>
import { BusView } from '@/api/business/business.js';
export default {
	data() {
		return {
			query:{
				b:{
					businessName:'',
					businessSource:'',
					businessType:'',
					salesAmount:'',
					winrate:'',
					businessStage:'',
					checkTime:'',
					remarks:'',
					clientName:'',
					realName:'',
					systemUserName:'',
					systemUserUuid:'',
				},

			}
		};
	},
	methods: {
		back() {
			this.$router.go(-1);
		},
		quxiao() {
			uni.navigateBack({
				delta: 1,
				animationDuration: 200
			});
		}
	},
	onLoad(opt) {
		let data = opt.businessUuid;
		BusView(data).then(res => {
			if(res.data.code==200){
				this.query.b=res.data.data[0];
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
