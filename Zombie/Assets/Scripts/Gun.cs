using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    private void Awake() {
        // 사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;

        bulletLineRenderer.enabled = false;
    }

    private void OnEnable() {
        // 총 상태 초기화
        ammoRemain = gunData.startAmmoRemain;

        magAmmo = gunData.magCapacity;

        state = State.Ready;

        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire() {
        //현재 상태가 발사 가능한 상태
        //&& 마지막 총 발사 시점에서 gunData.timeBetFire이상의 시간이 지남
        if(state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire) {
            lastFireTime = Time.time;

            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot() {
        //레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        //탄알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance)) {
            //어떤 물체와 충돌한 경우

            //충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null) {

                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }
        else {
            //레이가 다른 물체와 충돌하지 않았다면
            //탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;

        }

        //발사 이펙트 재생 시작
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if(magAmmo <= 0) {
            //탄창에 남은 탄알이 없다면 Empty
            state = State.Empty;
        }
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition) {
        //총구 화염 효과 재생
        muzzleFlashEffect.Play();

        //탄피 배출 효과 재생
        shellEjectEffect.Play();

        //총격 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        //선의 시작점
        bulletLineRenderer.SetPosition(0, fireTransform.position);

        //선의 끝점
        bulletLineRenderer.SetPosition(1, hitPosition);
        
        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() {
        if(state == State.Reloading || ammoRemain <=0 || magAmmo >= gunData.magCapacity) {
            //이미 재장전 중이거나 남은 탄알이 없거나
            //탄창에 탄알이 이미 가득한 경우 재장전 불가
            return false;
        }

        //재장전 처리 시작
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        //재장전 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);

        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        //탄창에 채울 탄알 계산
        int ammoToFill = gunData.magCapacity - magAmmo;

        //탄창에 채워야할 탄알이 남은 탄알보다 많다면
        //채워야 할 탄알 수를 남은 탄알 수에 맞춰 줄임
        
        if(ammoRemain < ammoToFill) {
            ammoToFill = ammoRemain;
        }
        //탄창을 채움
        magAmmo += ammoToFill;

        //남은 탄알에서 탄창에 채운만큼 뺌
        ammoRemain -= ammoToFill;

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}