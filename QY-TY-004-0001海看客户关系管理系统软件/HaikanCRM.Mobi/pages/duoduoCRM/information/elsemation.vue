<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" >
			<block slot="backText">返回</block>
			<block slot="content">个人信息</block>
		</cu-custom>
		<form>
			<view class="cu-card article no-card" style="height: 225px;">
				<view class="cu-item shadow-warp bg" style="height: 100%;">
					<view class="title">
						<view class="cu-avatar xl round bg-white" style="height: 100px;width: 100px;margin-top: 65px;margin-left: 35%;">
							<text class="avatar-text tt">{{realName}}</text>
						</view>
						<view class="radius" style="margin-top: 6px;margin-left: 35%;">
							<text class="cuIcon-people"></text>：个人信息
						</view>
					</view>
				</view>
			</view>
			<view class="cu-list ">
				<view class="cu-form-group inputlist">
					<view class="title">部门</view>
					<input name="input" style="text-align:right" disabled="disabled" v-model="list.types==null?'暂未加入部门':list.types"/>
				</view>
				<view class="cu-form-group inputlist">
					<view class="title">性别</view>
					<input name="input" style="text-align:right" disabled="disabled" v-model="list.sex"/>
				</view>
				<view class="cu-form-group inputlist">
					<view class="title">手机</view>
					<input name="input" style="text-align:right" disabled="disabled" v-model="list.phone==null?'暂未绑定手机':list.phone"/>
				</view>
				<view class="cu-form-group inputlist">
					<view class="title">邮箱</view>
					<input name="input" style="text-align:right" disabled="disabled" v-model="list.oldCard==null?'暂未绑定邮箱':list.oldCard"/>
				</view>
				<view class="cu-form-group inputlist">
					<view class="title">状态</view>
					<input name="input" style="text-align:right" disabled="disabled" v-model="list.zaiGang"/>
				</view>
				<view class="cu-form-group inputlist">
					<view class="title">注册时间</view>
					<input name="input" style="text-align:right" disabled="disabled" v-model="list.addTime"/>
				</view>
			</view>
			<view class="cu-list " style="height: 170px;">
				<view class="cu-avatar round lg margin-xs bg-olive"  style="margin: 10px;margin-left: 20px;">
					<text class="avatar-text">{{clientnum}}</text>
				</view>
				<view class="cu-avatar round lg margin-xs bg-blue"  style="margin: 10px;">
					<text class="avatar-text">{{businessnum}}</text>
				</view><br/>
					<text class="avatar-text text-grey" style="margin-left: 30px;">客户</text>
					<text class="avatar-text text-grey" style="margin-left: 40px;">商机</text>
			</view>
		</form>
	</view>
</template>

<script>
import { PersonalList } from '@/api/information/information.js';
	export default {
		data() {
			return {
				businessnum:0,
				clientnum:0,
				realName:'',
				list:{
					addTime:'',
					oldCard:'',
					phone:'',
					realName:'',
					sex:'',
					types:'',
					zaiGang:''
				}
			};
		},
		methods:{
			back() {
				this.$router.go(-1);
			}
		},
		onLoad(opt){
			console.log(opt)
			let data=opt.SystemUserUuid;
			PersonalList(data).then(res=>{
				this.businessnum=res.data.data.businessnum;
				this.clientnum=res.data.data.clientnum;
				this.realName=res.data.data.query.realName;
				this.list=res.data.data.query;
			});
		}
	}
</script>

<style>
	.inputlist{
			height: 50px;
			}
	
	.card {
		width: 90%;
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
		width: 100px;
	}
	
	.bg {
		background-image: url(@/static/image/bg-lan.png);
		background-position: 50%;
		background-size: cover;
		box-shadow: 0 0 1px 0 rgba(0, 0, 0, 0.1);
	}
	
	.cu-card.article > .cu-item .title {
		line-height: 80rpx;
		color: #ffffff;
		font-weight: 400;
	}
	
	.cu-list.menu-avatar > .cu-item .content {
		left: 30rpx;
		line-height: 60rpx;
	}
	
	.cu-list.menu-avatar > .cu-item {
		height: 280rpx;
	}
</style>
