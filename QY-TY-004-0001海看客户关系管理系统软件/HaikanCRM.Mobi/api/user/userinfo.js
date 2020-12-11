import http from '@/components/utils/http.js'

// editpwd
export const edituserPwd = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/rbac/user/edituserpwd',
    method: 'post',
  },data)
}