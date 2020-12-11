<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue"><block slot="content">通讯录</block></cu-custom>
		<view class="cu-bar search bg-white" style="margin-top:5px;">
			<view class="search-form round">
				<text class="cuIcon-search"></text>
				<input  type="text" placeholder="搜索成员" confirm-type="search" v-model="name"/>
			</view>
			<view class="action"><button class="cu-btn bg-blue shadow-blur round" style="background-image: none;background-color: rgb(0,129,255);" @click="selcom">搜索</button></view>
		</view>
		<view class="cu-list menu" style="margin-top: 10px;">
		<view class="cu-item arrow" >
			<button class="cu-btn content" open-type="contact" @click="clienman">
				<text class="cuIcon-my text-olive"></text>
				<text class="text-grey">客户联系人</text>
			</button>
		</view>
		</view>
		<scroll-view scroll-y class="indexes" :scroll-into-view="'indexes-' + listCurID" :scroll-with-animation="true" :enable-back-to-top="true">
			<block v-for="(item, index) in list" :key="index">
				<view :class="'indexItem-' + item.name" :id="'indexes-' + item.name" :data-index="item.name">
					<view class="cu-list menu-avatar no-padding">
						<view class="cu-item" style="height: 1.4rem;line-height: 1.6em;">
							<view class="cu-avatar round lg"  @click="infoclick(item.systemUserUuid)">{{ item.name }}</view>
							<view class="content" style="left:1.46rem;">
								<view class="text-grey"  @click="infoclick(item.systemUserUuid)">{{ item.realName }}</view>
								<!-- <view class="text-gray text-sm">
									{{item.companyName}}
								</view> -->
							</view>
							
							<view class="cu-avatar round lg margin-xs bg-gray"  style="left:5.96rem;top:7px;" @click="conLog(item.systemUserUuid)">
								<text class="avatar-text">{{item.txlu}}</text>
							</view>

								
						</view>				
					</view>
				</view>
			</block>
		</scroll-view>
	</view>
</template>

<script>
import { UserList } from '@/api/communList/communList.js';
export default {
	data() {
		return {
			StatusBar: this.StatusBar,
			CustomBar: this.CustomBar,
			listCurID: '',
			list: [],
			listCur: '',
			name:''
		};
	},
	mounted() {
		let data=this.name;
		console.log(data)
		this.doUserList(data);
	},
	onReady() {},
	methods: {
		selcom(){
			let data=this.name;
			console.log(data)
			this.doUserList(data);
		},
		doUserList(data){
			UserList(data).then(res => {
				console.log(123132);
				console.log(res);
				let list = [{}];
				for (let i = 0; i < res.data.data.length; i++) {
					list[i] = {};
					list[i].name = res.data.data[i].realName.substring(0, 1);
					list[i].realName = res.data.data[i].realName;
					list[i].txlu = res.data.data[i].txlu;
					list[i].systemUserUuid = res.data.data[i].systemUserUuid;
				}
				this.list = list;
				this.listCur = list[0];
			});
		},
		infoclick(e) {
			uni.navigateTo({
				url: '/pages/duoduoCRM/information/elsemation?SystemUserUuid=' + e
			});
		},
		conLog(e){
			uni.navigateTo({
				url: '/pages/duoduoCRM/calllog/calllog?logUuid=' + e
			});
		},
		clienman() {
			let data = getApp().globalData.SystemUserUuid;
			uni.navigateTo({
				url: '/pages/duoduoCRM/clien/clien?clienUuid=' + data
			});
		}
	}
};
</script>

<style>
	.indexes {
		position: relative;
		height: 460px;
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
</style>
