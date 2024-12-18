import axios from "axios";
import { useMemo } from "react";

export function useAxios() {
  return useMemo(() => {
    return axios.create({
      baseURL: 'http://localhost:8080',
      withCredentials: true
    });
  }, [])
}