FROM envoyproxy/envoy:v1.17-latest

COPY ./Envoy.yaml /etc/envoy/envoy.yaml

CMD /usr/local/bin/envoy -c /etc/envoy/envoy.yaml