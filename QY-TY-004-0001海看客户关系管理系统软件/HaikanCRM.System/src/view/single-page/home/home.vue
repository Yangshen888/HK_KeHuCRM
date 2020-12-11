<template>
  <div>
    <Row :gutter="10">
      <i-col
        :xs="12"
        :md="8"
        :lg="4"
        v-for="(infor, i) in inforCardData"
        :key="`infor-${i}`"
        style="height: 120px; padding-bottom: 10px"
      >
        <infor-card
          shadow
          :color="infor.color"
          :icon="infor.icon"
          :icon-size="36"
        >
          <count-to :end="infor.count" count-class="count-style" />
          <p>{{ infor.title }}</p>
        </infor-card>
      </i-col>
    </Row>
    <Row :gutter="10" style="margin-top: 10px">
      <i-col :md="24" :lg="8" style="margin-bottom: 20px">
        <Card shadow>
          <div id="c4" style="height: 300px"></div>
          <!-- <chart-pie
            style="height: 300px"
            :value="pieData"
            text="客户地区分布"
            v-if="show"
          ></chart-pie> -->
        </Card>
      </i-col>
      <i-col :md="24" :lg="16" style="margin-bottom: 20px">
        <Card shadow>
          <!-- <chart-bar
            style="height: 300px"
            :value="barData"
            text="每周联系记录条数"
            v-if="show1"
          /> -->
          <div id="c3" style="height: 300px; width: 110%"></div>
        </Card>
      </i-col>
    </Row>
    <Row>
      <Card shadow>
        <!-- <example style="height: 310px" /> -->
        <div id="c5" style="height: 310px"></div>
      </Card>
    </Row>
  </div>
</template>

<script>
// 引入基本模板
let echarts = require("echarts/lib/echarts");
// 引入柱状图组件
require("echarts/lib/chart/bar");
require("echarts/lib/chart/line");
require("echarts/lib/chart/pie");

// 引入提示框和title组件
require("echarts/lib/component/tooltip");
require("echarts/lib/component/title");
require("echarts/theme/dark");
import tdTheme from "_c/charts/theme.json";
echarts.registerTheme("tdTheme", tdTheme);
import InforCard from "_c/info-card";
import CountTo from "_c/count-to";
import { ChartPie, ChartBar } from "_c/charts";
import Example from "./example.vue";
import {
  GetBaseInfo,
  GetCircle,
  GetBarChart,
  GetLineChart,
} from "@/api/IndexStatistics/IndexStatistics";
export default {
  name: "home",
  components: {
    InforCard,
    CountTo,
    ChartPie,
    ChartBar,
    Example,
  },
  data() {
    return {
      inforCardData: [
        {
          title: "客户总数",
          icon: "md-person-add",
          count: 0,
          color: "#2d8cf0",
        },
        {
          title: "联系人总数",
          icon: "md-locate",
          count: 0,
          color: "#19be6b",
        },
        {
          title: "商机总数",
          icon: "md-help-circle",
          count: 0,
          color: "#ff9900",
        },
        {
          title: "当月新增商机总数",
          icon: "md-share",
          count: 0,
          color: "#ed3f14",
        },
        {
          title: "当月联系记录总数",
          icon: "md-chatbubbles",
          count: 0,
          color: "#E46CBB",
        },
        {
          title: "当月客户生日总数",
          icon: "md-map",
          count: 0,
          color: "#9A66E4",
        },
      ],
      pieData: [
        // {value: 335, name: '直接访问'},
        // {value: 310, name: '邮件营销'},
      ],
      barData: {
        // Mon: 13253,
        // Tue: 34235,
        // Wed: 26321,
        // Thu: 12340,
        // Fri: 24643,
        // Sat: 1322,
        // Sun: 1324,
      },
      show: false,
      show1: false,
      show2: false,
    };
  },
  methods: {
    BaseInfo() {
      GetBaseInfo().then((res) => {
        console.log("首页基础数据统计");
        console.log(res);
        this.inforCardData[0].count = res.data.data[0].cusCount;
        this.inforCardData[1].count = res.data.data[0].conCount;
        this.inforCardData[2].count = res.data.data[0].busCount;
        this.inforCardData[3].count = res.data.data[0].busAddCount;
        this.inforCardData[4].count = res.data.data[0].callAddCount;
        this.inforCardData[5].count = res.data.data[0].conBirthCount;
      });
    },
    LineChart() {
      GetLineChart().then((res) => {
        console.log(2323);
        console.log(res);
        // 基于准备好的dom，初始化echarts实例
        let myChart = echarts.init(document.getElementById("c5"), "tdTheme");
        // 绘制图表
        let option = {
          xAxis: {
            type: "category",
            // data: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
            data: res.data.data.query,
          },
          yAxis: {
            type: "value",
          },
          series: [
            {
              // data: [820, 932, 901, 934, 1290, 1330, 1320],
              data: res.data.data.query2,
              type: "line",
            },
          ],
        };
        myChart.setOption(option);
      });
    },
    Circle() {
      GetCircle().then((res) => {
        let myChart = echarts.init(document.getElementById("c4"), "tdTheme");
        // 绘制图表
        let option = {
          title: {
            text: "客户地区分布",
            subtext: "省份分布统计",
            left: "center",
          },
          tooltip: {
            trigger: "item",
            formatter: "{a} <br/>{b} : {c} ({d}%)",
          },
          legend: {
            orient: "vertical",
            left: "left",
            data: res.data.data.query1,
          },
          series: [
            {
              name: "百分比",
              type: "pie",
              radius: "55%",
              center: ["50%", "60%"],
              data: res.data.data.query,
              emphasis: {
                itemStyle: {
                  shadowBlur: 10,
                  shadowOffsetX: 0,
                  shadowColor: "rgba(0, 0, 0, 0.5)",
                },
              },
            },
          ],
        };
        myChart.setOption(option);
      });
    },
    BarChart() {
      GetBarChart().then((res) => {
        // 基于准备好的dom，初始化echarts实例
        let myChart = echarts.init(document.getElementById("c3"), "tdTheme");
        // 绘制图表
        let option = {
          color: ["#3398DB"],
          tooltip: {
            trigger: "axis",
            axisPointer: {
              // 坐标轴指示器，坐标轴触发有效
              type: "shadow", // 默认为直线，可选为：'line' | 'shadow'
            },
          },
          grid: {
            containLabel: true,
          },
          xAxis: [
            {
              type: "category",
              //data: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
              data: res.data.data.timelist,
              axisTick: {
                alignWithLabel: true,
              },
              axisLabel: {
                interval: 0,
              },
            },
          ],
          yAxis: [
            {
              type: "value",
            },
          ],
          series: [
            {
              name: "本周联系记录条数",
              type: "bar",
              barWidth: "30%",
              //data: [10, 52, 200, 334, 390, 330, 220],
              data: res.data.data.sumlist,
            },
          ],
        };
        myChart.setOption(option);
      });
    },
  },
  mounted() {
    this.BaseInfo();
    this.Circle();
    this.BarChart();
    this.LineChart();
  },
};
</script>

<style lang="less">
.count-style {
  font-size: 50px;
}
</style>
