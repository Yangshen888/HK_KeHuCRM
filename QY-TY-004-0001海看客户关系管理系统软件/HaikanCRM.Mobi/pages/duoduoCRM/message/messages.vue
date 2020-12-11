<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue"><block slot="content">消息</block></cu-custom>
		<view class="cu-list menu-avatar">
			<view
				class="cu-item"
				:class="modalName == 'move-box-' + index ? 'move-cur' : ''"
				v-for="(item, index) in messagebox"
				:key="index"
				@touchstart="ListTouchStart"
				@touchmove="ListTouchMove"
				@touchend="ListTouchEnd"
				:data-target="'move-box-' + index"
				style="height: 1.4rem;"
			>
				<view class="content" style="left: 30px;">
					<view class="text-grey">{{ item.realName }}</view>
					<view class="text-gray text-sm">把客户{{ item.clientName }}分享给你</view>
				</view>
				<view class="action" style="width: 80px;">
					<view class="text-grey text-xs">{{ item.addTime1 }}</view>
					<view class="text-grey text-xs">{{ item.addTime2 }}</view>
				</view>
				<view class="move"><view class="bg-red" @click="MesDelete(item)">删除</view></view>
			</view>
		</view>
		
	</view>
</template>

<script>
import uniPopup from '@/components/uni-popup/uni-popup.vue';
import uniIcon from '@/components/uni-icon/uni-icon.vue';
import { MessageShow, MesDelete } from '@/api/message/message.js';
export default {
	components: {
		uniIcon,
		uniPopup
	},
	data() {
		return {
			modalName: null,
			listTouchStart: 0,
			messagebox: {
				addTime1: '',
				addTime2: '',
				clientName: '',
				realName: ''
			}
		};
	},
	mounted() {
		this.GetMessage();
	},
	methods: {
		GetMessage() {
			let that=this;
			let data = getApp().globalData.SystemUserUuid;
			MessageShow(data).then(res => {
				that.messagebox = res.data.data;
				for (let i = 0; i < this.messagebox.length; i++) {
					this.messagebox[i].addTime1 = this.messagebox[i].addTime.split(' ')[0];
					this.messagebox[i].addTime2 = this.messagebox[i].addTime.split(' ')[1].substring(0, 5);
				}
			});
		},
		MesDelete(e) {
			let that = this;
			uni.showModal({
				title: '提示',
				content: '确定删除此信息吗？',
				showCancel: true,
				success: function(rest) {
					if (rest.confirm) {
						let data = e.messageUuid;
						MesDelete(data).then(res => {
							console.log(res);
							if (res.data.code == 200) {
								uni.showToast({
									title: res.data.message,
									icon: 'none'
								});
								that.GetMessage();
								return;
							}
						});
					}
				}
			});
		},
		// ListTouch触摸开始
		ListTouchStart(e) {
			this.listTouchStart = e.touches[0].pageX;
		},

		// ListTouch计算方向
		ListTouchMove(e) {
			this.listTouchDirection = e.touches[0].pageX - this.listTouchStart > 0 ? 'right' : 'left';
		},

		// ListTouch计算滚动
		ListTouchEnd(e) {
			if (this.listTouchDirection == 'left') {
				this.modalName = e.currentTarget.dataset.target;
			} else {
				this.modalName = null;
			}
			this.listTouchDirection = null;
		}
	}
};
</script>

<style>
</style>
