<template>
	<view style="padding-top: 0px;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">商机列表</block>
		</cu-custom>
		<view class="container999">
			<view class="cu-bar search bg-white" style="margin-top:5px;">
				<view class="search-form round">
					<text class="cuIcon-search"></text>
					<input v-model="stores.pogoda.BusinessName" type="text" placeholder="商机名称" confirm-type="search" />
				</view>
				<view class="action"><button class="cu-btn bg-blue shadow-blur round" @click="search">搜索</button></view>
			</view>
			<view class="cu-form-group" style="display: flex;background-color: #f6f6f6;margin: 10px 70px;text-align: center;margin-bottom: -10px;">
				<view style="flex: 1;height: 30px;line-height: 30px;border-radius: 15px;background-color: #fff;">
					<picker @change="changeType" :value="indexType" :range="pickerType">
						<view class="picker">{{ indexType > -1 ? pickerType[indexType] : '请选择商机阶段' }}</view>
					</picker>
				</view>
				<view @click="clearChoose" style="font-size: 15px;background-color: #0294FA;color: #fff;padding: 5px 10px;border-radius: 10px;margin-left: 10px;">清空</view>
			</view>
			<view class="cu-item" v-for="(item, index) in kw" v-if="kw.length > 0" :key="index" style="width: 100%;">
				<view class="content" style="width: 100%;">
					<view class="card">
						<view>
							<text style="color: #0081FF;">商机:</text>
							{{ item.businessName }}
						</view>
						<view>
							<text style="color: #0081FF;">所属客户:</text>
							{{ item.clientName }}
						</view>
<!-- 						<view>
							<text style="color: #0081FF;">客户经理:</text>
							{{ item.clientManager }}
						</view> -->
						<view >
							 
							<view class="cu-tag bg-yellow sm round" @click="clickPogodaID(item.businessUuid)">详情</view>
							<view class="cu-tag bg-green sm round" @click="clickEditID(item.businessUuid)" >编辑</view>
						</view>
					</view>
				</view>
			</view>
		</view>
		<button class="cu-btn cuIcon bg-green round lg" style="position: fixed;bottom: 0;right: 30px;top:500px;" @click="geturl">
			<text class="cuIcon-add shadow"></text>
		</button>
	</view>
</template>

<script>
import uniPopup from '@/components/uni-popup/uni-popup.vue';
import uniIcon from '@/components/uni-icon/uni-icon.vue';
import { BusShow } from '@/api/business/business.js';
export default {
	components: {
		uniIcon,
		uniPopup
	},
	data() {
		return {
			stores: {
				pogoda: {
					ClientUUID: '',
					BusinessName: '',
					kw: '',
					Kw1:'',
					SystemUserUuid:''
				}
			},
			pickerType: ['沟通调研', '方案策划', '招投标', '合同签订', '已关闭'],
			indexType: -1,
			kw: []
		};
	},
	methods: {
		clearChoose(){
			this.stores.pogoda.Kw1 = "",
			this.indexType=-1;
			this.search();
		},
		changeType(e) {
			this.indexType = e.detail.value;
			this.stores.pogoda.Kw1 = this.pickerType[e.detail.value];
			this.search();
			//this.mobile.businessType = this.pickerType[this.indexType];
		},
		back() {
			if (this.stores.pogoda.ClientUUID == '') {
				uni.navigateTo({
					url: '/pages/duoduoCRM/gognneng/gognneng'
				});
			} else {
				this.$router.go(-1);
			}
		},
		geturl() {
			uni.navigateTo({
				url: '/pages/duoduoCRM/business/addbusiness'
			});
		},
		search() {//JSON.stringify(this.stores.pogoda)
			BusShow(JSON.stringify(this.stores.pogoda)).then(res => {
				if (res.data.data == '') {
					uni.showToast({
						title: '暂未查到商机信息',
						icon: 'none'
					});
					this.kw = res.data.data;
				} else {
					this.kw = res.data.data;
				}
			});
		},
		clickPogodaID(id) {
			uni.navigateTo({
				url: '/pages/duoduoCRM/business/infobusiness?businessUuid=' + id+"&ClientUuid="+this.stores.pogoda.ClientUuid
			});
		},
		clickEditID(id) {
			uni.navigateTo({
				url: '/pages/duoduoCRM/business/addbusiness?businessUuid=' + id+"&ClientUuid="+this.stores.pogoda.ClientUuid
			});
		}
	},
	onLoad(opt) {
		if(opt.clientUuid!=undefined){
			this.stores.pogoda.ClientUuid = opt.clientUuid;
		}
		 // getApp().globalData.SystemUserUuid
		this.stores.pogoda.SystemUserUuid =getApp().globalData.SystemUserUuid;
		console.log(this.stores.pogoda)
		this.search();
	}
};
</script>
<style>

.indexes {
	position: relative;
}

.indexBar {
	position: fixed;
	right: 0px;
	bottom: 0px;
	padding: 20upx 20upx 20upx 60upx;
	display: flex;
	align-items: center;
}

.indexBar .indexBar-box {
	width: 40upx;
	height: auto;
	background: #fff;
	display: flex;
	flex-direction: column;
	box-shadow: 0 0 20upx rgba(0, 0, 0, 0.1);
	border-radius: 10upx;
}

.indexBar-item {
	flex: 1;
	width: 40upx;
	height: 40upx;
	display: flex;
	align-items: center;
	justify-content: center;
	font-size: 24upx;
	color: #888;
}

movable-view.indexBar-item {
	width: 40upx;
	height: 40upx;
	z-index: 9;
	position: relative;
}

movable-view.indexBar-item::before {
	content: '';
	display: block;
	position: absolute;
	left: 0;
	top: 10upx;
	height: 20upx;
	width: 4upx;
	background-color: #f37b1d;
}

.indexToast {
	position: fixed;
	top: 0;
	right: 80upx;
	bottom: 0;
	background: rgba(0, 0, 0, 0.5);
	width: 100upx;
	height: 100upx;
	border-radius: 10upx;
	margin: auto;
	color: #fff;
	line-height: 100upx;
	text-align: center;
	font-size: 48upx;
}

.container999 {
	width: 100vw;
	font-size: 28upx;
	min-height: 100vh;
	overflow: hidden;
	color: #6b8082;
	position: relative;
	/* top:50px; */
	background-color: #f6f6f6;
}

.content {
	width: 100%;
}

.card {
	width: 80%;
	background-color: white;
	margin: 40rpx auto 42upx auto;
	background: #ffffff;
	box-shadow: 0 0 5px 0 rgba(0, 0, 0, 0.1);
	border-radius: 5px;
	position: relative;
	padding: 40upx;
	line-height: 50upx;
}

.tt {
	font-size: 30upx;
	font-weight: bold;
}
</style>
