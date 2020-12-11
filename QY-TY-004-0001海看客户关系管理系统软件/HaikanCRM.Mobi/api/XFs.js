import http from '@/components/utils/http.js'

export const getXFsList = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/base/rubbish/List',
    method: 'post',
  },data)
}
